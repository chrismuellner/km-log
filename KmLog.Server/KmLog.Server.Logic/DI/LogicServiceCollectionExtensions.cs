using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace KmLog.Server.Logic.DI
{
    public static class LogicServiceCollectionExtensions
    {
        public static IServiceCollection AddLogic(this IServiceCollection services)
        {
            services.AddScoped<FuelActionLogic>();
            services.AddScoped<CarLogic>();

            return services;
        }
    }
}
