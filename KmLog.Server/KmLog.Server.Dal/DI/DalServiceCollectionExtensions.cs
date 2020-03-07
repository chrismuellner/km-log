using Microsoft.Extensions.DependencyInjection;

namespace KmLog.Server.Dal.DI
{
    public static class DalServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRefuelEntryRepository, RefuelEntryRepository>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
