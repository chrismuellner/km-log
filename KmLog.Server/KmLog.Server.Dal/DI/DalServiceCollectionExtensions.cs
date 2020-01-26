using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace KmLog.Server.Dal.DI
{
    public static class DalServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRefuelActionRepository, RefuelActionRepository>();
            services.AddScoped<ICarRepository, CarRepository>();

            return services;
        }
    }
}
