using Microsoft.Extensions.DependencyInjection;

namespace KmLog.Server.Logic.DI
{
    public static class LogicServiceCollectionExtensions
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddScoped<EntryLogic>();
            services.AddScoped<CarLogic>();
            services.AddScoped<AuthenticationLogic>();
            services.AddScoped<UserLogic>();

            return services;
        }
    }
}
