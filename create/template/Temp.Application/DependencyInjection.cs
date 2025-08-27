using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Temp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(
                filter => filter.Where(x =>
                    x.Name.EndsWith("Service") || x.Name.EndsWith("Services") || x.Name.EndsWith("Validation")),
                publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }
}
