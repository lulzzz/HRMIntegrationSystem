using Absence.Api.Domain.Interfaces;
using Absence.Api.Domain.Services;
using Common.Api.ProxyClient.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Absence.Api.Extensions
{
    public static class IoCExtension
    {
        public static void AddAbsenceIocMapping(this IServiceCollection services)
        {
            services.AddScoped<IStatisticsService, StatisticsService>();
        }
        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCommonHttpClient(configuration);
        }
    }
}
