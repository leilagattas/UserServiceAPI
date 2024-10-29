using Microsoft.Extensions.DependencyInjection;
using UserServiceAPI.Services.Interfaces;
using UserServiceAPI.Services.Services;

namespace UserServiceAPI.Services
{
    public static class DependencyInjectionServices
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
