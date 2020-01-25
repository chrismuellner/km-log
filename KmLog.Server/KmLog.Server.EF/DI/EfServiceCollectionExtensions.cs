using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KmLog.Server.EF.DI
{
    public static class EfServiceCollectionExtensions
    {
        private const string CONNECTION_STRING = "KmLogContext";

        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ => new KmLogContext(configuration.GetConnectionString(CONNECTION_STRING)));

            return services;
        }
    }
}
