# dotnet-cleanarch-builder

Generate a production-ready **.NET 9** solution with a clean architecture layout in seconds.

> This repo contains the **tool** that scaffolds a new solution from your template sources.

---

## Prerequisites

- **Node.js ≥ 22** (to run the tool via `npx`)

---

## Quick Start

1) **Clone and enter the repo**
```bash
git clone <repository_url>
cd <project_directory>
```

2) **Generate a new solution**
```bash
# macOS/Linux
npx -y ./create <SolutionName> --kebab-root

# Windows (PowerShell)
npx -y ./create <SolutionName> --kebab-root

# Example
npx -y ./create Contoso --kebab-root
```

- `-y` — optional; auto-accepts any prompts from `npx`  
- `--kebab-root` — optional; creates the root folder in kebab-case (e.g., `Contoso` → `contoso`)

---

## What You Get

- A .NET 9 solution pre-organized for **Clean Architecture**
- Consistent naming and folder structure
- Ready-to-build projects and sensible defaults

> After generation, open the solution in your IDE and run the API/project as usual.

### 📚 Project structure

This project follows the Domain-Driven Design (DDD) approach, which helps organize the code into distinct layers that focus on different concerns of the application.

#### 📔 Temp.API

The `Temp.API` project is an **ASP.NET Core API** and serves as the entry point for the application. It contains the web API layer that handles HTTP requests, controllers, routing, and middlewares. This project is responsible for exposing endpoints to the client.

#### 📕 Temp.Application

The `Temp.Application` is a **Class Library** that contains the application's business logic and service layer. It contains the **Application Services, and DTOs**.

#### 📗 Temp.Domain

The `Temp.Domain` is a **Class Library** that defines the core business logic of the application. Including the **Domain Entities, Models, Interfaces, and Exceptions, etc**.

#### 📘 Temp.EFCore

The `Temp.EFCore` is a **Class Library** contains the **Database Context, Configurations, and Migrations**. It defines how the application interacts with the database using **Entity Framework Core**.

#### 📙 Temp.Infrastructure

The `Temp.Infrastructure` is a **Class Library** that provides implementations for external dependencies and services. It includes infrastructure concerns such as **Data Repositories, Redis Caching, Email Services, etc**.

---

## Troubleshooting

- **“command not found”**: Ensure you’re in the repo root where `create` lives.  
- **Windows path issues**: Use `./create` instead of `.\create`.  
- **Node version**: Confirm `node -v` is ≥ 22 if using `npx`.

---

## License

MIT (see `LICENSE`).
