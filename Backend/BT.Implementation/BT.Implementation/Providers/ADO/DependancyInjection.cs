using Microsoft.Extensions.DependencyInjection;
using BT.Implementation.Providers.Interfaces;
using BT.Implementation.Providers.ADO;
using BT.Implementation.Services;
using BT.Interfaces.Services;


public static class DependencyInjection
{
    public static IServiceCollection AddImplementation(
        this IServiceCollection services)
    {
        // Register providers
        services.AddScoped<IBugProvider, BugProvider>();
        services.AddScoped<IAuthProvider, AuthProvider>();

        // Register services
        services.AddScoped<IBugService, BugService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
