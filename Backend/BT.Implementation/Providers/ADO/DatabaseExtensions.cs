using BT.Implementation.Providers.ADO;
using BT.Implementation.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace BT.Implementation.Providers.ADO;

public static class DatabaseExtensions
{
    public static IServiceCollection AddDatabase(
    this IServiceCollection services)
    {
        services.AddSingleton<DatabaseConnection>();
        services.AddScoped<IBugProvider, BugProvider>();
        services.AddScoped<IAuthProvider, AuthProvider>();
        return services;
    }
}