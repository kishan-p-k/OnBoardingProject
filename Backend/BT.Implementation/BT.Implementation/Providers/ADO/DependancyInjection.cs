using Microsoft.Extensions.DependencyInjection;
using BT.Implementation.Providers.Interfaces;
using BT.Implementation.Providers.ADO;
using BT.Implementation.Services;
using BT.Interfaces.Services;

namespace BT.Implementation.Providers.ADO
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddImplementation(
            this IServiceCollection services)
        {
            // Register providers
            services.AddScoped<IBugProvider, BugProvider>();
            services.AddScoped<IAuthProvider, AuthProvider>();
            services.AddScoped<IBugDetailProvider, BugDetailProvider>();
            services.AddScoped<IUserBugsProvider, UserBugsProvider>();

            // Register services
            services.AddScoped<IBugService, BugService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBugDetailService, BugDetailService>();
            services.AddScoped<IUserBugsService, UserBugsService>();
            return services;
        }
    }
}
