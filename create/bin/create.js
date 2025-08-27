#!/usr/bin/env node
/* eslint-disable no-console */
const fs = require('fs');
const path = require('path');

function usage() {
  console.log(`
Generate a new .NET solution from the bundled template by changing the project/namespace prefix.

Usage:
  npx -y ./create <SolutionPrefix> [--from Temp] [--dir .] [--kebab-root]

Options:
  <SolutionPrefix>          New prefix for project & namespace (e.g., Contoso or Contoso.Booking)
  --from <Prefix>           Existing prefix in the template to replace (default: Temp)
  --dir <Path>              Destination directory (default: current working directory)
  --kebab-root              Name the root folder as kebab-case of the prefix (default: use prefix directly)

Examples:
  npx -y ./create Contoso --kebab-root
  npx -y ./create MyCo.Booking --from Temp --dir ./output
`);
}

function parseArgs(argv) {
  const args = argv.slice(2);
  if (!args.length || args.includes('--help') || args.includes('-h'))
    return { help: true };
  let prefix = null;
  let from = 'Temp';
  let dir = process.cwd();
  let kebabRoot = false;
  for (let i = 0; i < args.length; i++) {
    const a = args[i];
    if (!a.startsWith('--') && !prefix) {
      prefix = a;
    } else if (a === '--from') {
      from = args[++i];
    } else if (a === '--dir') {
      dir = path.resolve(args[++i]);
    } else if (a === '--kebab-root') {
      kebabRoot = true;
    }
  }
  return { help: !prefix, prefix, from, dir, kebabRoot };
}

function kebabCase(s) {
  // Replace dots with dashes, spaces to dashes, camel boundaries to dashes
  return s
    .replace(/[\s_]+/g, '-')
    .replace(/\./g, '-')
    .replace(/([a-z0-9])([A-Z])/g, '$1-$2')
    .toLowerCase();
}

function cp(src, dest) {
  const st = fs.statSync(src);
  if (st.isDirectory()) {
    if (!fs.existsSync(dest)) fs.mkdirSync(dest, { recursive: true });
    for (const n of fs.readdirSync(src)) {
      cp(path.join(src, n), path.join(dest, n));
    }
  } else {
    fs.copyFileSync(src, dest);
  }
}

function walk(dir) {
  /** @type {string[]} */
  const all = [];
  (function rec(p) {
    for (const name of fs.readdirSync(p)) {
      const full = path.join(p, name);
      const st = fs.statSync(full);
      all.push(full);
      if (st.isDirectory()) rec(full);
    }
  })(dir);
  return all;
}

function renamePrefixInPaths(root, from, to) {
  // Rename files and directories bottom-up when names start with `${from}.`
  const all = walk(root).sort((a, b) => b.length - a.length);
  let count = 0;
  for (const p of all) {
    const base = path.basename(p);
    let newBase = base;
    if (base.startsWith(from + '.')) {
      newBase = to + base.slice(from.length);
    } else if (base === from + '.sln') {
      newBase = to + '.sln';
    }
    if (newBase !== base) {
      const target = path.join(path.dirname(p), newBase);
      fs.renameSync(p, target);
      count++;
    }
  }
  return count;
}

function replaceAllInFile(file, from, to, kebabFrom, kebabTo) {
  // Targeted replacements across several languages / file types
  let text = fs.readFileSync(file, 'utf8');
  const orig = text;

  // using / global using / @using
  text = text.replace(
    /^(\s*(?:global\s+)?using\s+(?:static\s+)?)\b(?:${FROM})\./gm,
    `$1${to}.`.replace('${FROM}', from)
  );
  text = text.replace(
    /^(\s*@using\s+)\b(?:${FROM})\./gm,
    `$1${to}.`.replace('${FROM}', from)
  );

  // namespace declarations
  text = text.replace(
    /(^\s*namespace\s+)\b(?:${FROM})(?=[\s.;{])/gm,
    `$1${to}`.replace('${FROM}', from)
  );

  // ProjectReference paths
  text = text.replace(
    /(Include="[^"]*)\b(?:${FROM})\./g,
    `$1${to}.`.replace('${FROM}', from)
  );

  // RootNamespace / AssemblyName / DefaultNamespace
  text = text.replace(
    /(<RootNamespace>)\b(?:${FROM})(?=[\s.<])/g,
    `$1${to}`.replace('${FROM}', from)
  );
  text = text.replace(
    /(<AssemblyName>)\b(?:${FROM})(?=[\s.<])/g,
    `$1${to}`.replace('${FROM}', from)
  );
  text = text.replace(
    /(<DefaultNamespace>)\b(?:${FROM})(?=[\s.<])/g,
    `$1${to}`.replace('${FROM}', from)
  );

  // Generic dotted prefix anywhere: Temp.XXX -> NewPrefix.XXX
  const prefixDot = new RegExp(
    '\\b' + from.replace(/[.*+?^${}()|[\]\\]/g, '\\$&') + '\\.',
    'g'
  );
  text = text.replace(prefixDot, to + '.');
  // Also replace plain prefix tokens (e.g., "Temp" in README) with word boundaries
  const prefixWord = new RegExp(
    '\\b' + from.replace(/[.*+?^${}()|[\\]\\\\]/g, '\\\\$&') + '\\b',
    'g'
  );
  text = text.replace(prefixWord, to);

  // Legacy: replace EMS.* and plain EMS tokens too, to support older templates
  text = text.replace(/\bEMS\./g, to + '.');
  text = text.replace(/\bEMS\b/g, to);

  // Optional kebab root name swaps (e.g., easy-booking -> contoso)
  if (kebabFrom && kebabTo && kebabFrom !== kebabTo) {
    const kebabRe = new RegExp(
      kebabFrom.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'),
      'g'
    );
    text = text.replace(kebabRe, kebabTo);
  }

  if (text !== orig) {
    fs.writeFileSync(file, text, 'utf8');
    return true;
  }
  return false;
}

function main() {
  const args = parseArgs(process.argv);
  if (args.help) return usage();

  const { prefix: toPrefix, from: fromPrefix, dir, kebabRoot } = args;
  const kebabFrom = kebabCase(fromPrefix);
  const kebabTo = kebabCase(toPrefix);

  const templateDir = path.resolve(__dirname, '..', 'template');
  if (!fs.existsSync(templateDir)) {
    console.error('Template not found:', templateDir);
    process.exit(1);
  }

  // Decide output root dir name
  const outRootName = kebabRoot ? kebabTo : toPrefix;
  const outRoot = path.join(dir, outRootName);
  if (fs.existsSync(outRoot)) {
    console.error('✖ Destination already exists:', outRoot);
    process.exit(1);
  }
  fs.mkdirSync(outRoot, { recursive: true });

  // 1) Copy template
  cp(templateDir, outRoot);

  // 2) Rename paths (files/dirs) that start with fromPrefix.
  const pathRenames = renamePrefixInPaths(outRoot, fromPrefix, toPrefix);

  // 3) Replace inside files
  const exts = new Set([
    '.cs',
    '.csx',
    '.cshtml',
    '.razor',
    '.sln',
    '.csproj',
    '.props',
    '.targets',
    '.json',
    '.md',
    '.yml',
    '.yaml',
    '.xml',
    '.editorconfig',
  ]);
  const files = walk(outRoot).filter((p) => fs.statSync(p).isFile());
  let fileChanges = 0;
  let replacements = 0;
  for (const f of files) {
    if (!exts.has(path.extname(f))) continue;

    const before = fs.readFileSync(f, 'utf8');
    const changed = replaceAllInFile(
      f,
      fromPrefix,
      toPrefix,
      kebabFrom,
      kebabTo
    );
    if (changed) {
      fileChanges++;
      const after = fs.readFileSync(f, 'utf8');
      // crude count
      const re = new RegExp(fromPrefix, 'g');
      const reK = new RegExp(kebabFrom, 'g');
      const c1 =
        (before.match(re) || []).length - (after.match(re) || []).length;
      const c2 =
        (before.match(reK) || []).length - (after.match(reK) || []).length;
      replacements += Math.max(0, c1 + c2);
    }
  }

  console.log('✔ Generated:', outRoot);
  console.log(`  Path renames: ${pathRenames}`);
  console.log(
    `  File changes: ${fileChanges} (approx replacements: ${replacements})`
  );
  console.log('\nNext:');
  console.log(`  cd ${outRootName}`);
  console.log('  dotnet build');
  console.log('  # Run your web project if present, e.g.:');
  console.log(`  dotnet run --project src/WebApi/${toPrefix}.WebApi.csproj`);
}

main();
