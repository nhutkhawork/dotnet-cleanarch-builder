using Temp.Domain.Entities.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Temp.EFCore;

public class AppDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AppDbContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override int SaveChanges()
    {
        UpdateAuditableEntities();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities()
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();
        var now = DateTime.UtcNow;

        var httpContext = _httpContextAccessor.HttpContext;
        var userName = httpContext?.Items["preferred_username"] ?? "";

        //var user = Users.FirstOrDefault(x => x.UserName.Equals(userName));

        //foreach (var entry in entries)
        //{
        //    if (entry.State == EntityState.Added)
        //    {
        //        entry.Entity.CreatedAt = now;
        //        entry.Entity.ModifiedAt = now;

        //        entry.Entity.CreatedBy = user;
        //        entry.Entity.CreatedById = user?.Id;

        //        entry.Entity.ModifiedBy = user;
        //        entry.Entity.ModifiedById = user?.Id;
        //    }
        //    else if (entry.State == EntityState.Modified)
        //    {
        //        entry.Entity.ModifiedAt = now;
        //        entry.Entity.ModifiedBy = user;
        //        entry.Entity.ModifiedById = user?.Id;

        //        if (entry.Property(x => x.CreatedAt).IsModified)
        //        {
        //            entry.Property(x => x.CreatedAt).IsModified = false;
        //            entry.Property(x => x.CreatedById).IsModified = false;
        //        }
        //    }
        //}
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
