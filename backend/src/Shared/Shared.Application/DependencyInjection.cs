using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreApplication(this IServiceCollection services)
    {
        return services;
    }
}