using AutoMapper;
using KmLog.Server.Logic.Profiles;
using Microsoft.Extensions.DependencyInjection;

namespace KmLog.Server.Logic.DI
{
    public static class AutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DtoMappingProfile>();
                cfg.AddProfile<EntityMappingProfile>();
            });
            
            services.AddSingleton(_ => config.CreateMapper());
            return services;
        }
    }
}
