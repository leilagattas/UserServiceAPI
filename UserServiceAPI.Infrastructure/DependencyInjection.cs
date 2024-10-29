using Microsoft.Extensions.DependencyInjection;
using UserServiceAPI.Infrastructure.Interfaces;
using UserServiceAPI.Infrastructure.Repository;

namespace UserServiceAPI.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

    }
}
