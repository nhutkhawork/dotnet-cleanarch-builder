using Temp.Domain.Interfaces.DataAccess;
using Temp.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Temp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDataAccessInfrastructure();
        services.AddRepositories();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(
                filter => filter.Where(x => x.Name.EndsWith("Repository")),
                publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsMatchingInterface()
            .WithScopedLifetime());


        return services;
    }

    public static IServiceCollection AddDataAccessInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<DbFactory>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
