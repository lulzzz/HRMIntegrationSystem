using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Services.Extensions
{
    public static class AutomapperExtension
    {
        public static void AddAutoMapper(this IServiceCollection services, MapperConfiguration config)
        {
            config.AssertConfigurationIsValid();
            services.AddSingleton(config);
            services.AddScoped(sp => config.CreateMapper());
        }
    }
}