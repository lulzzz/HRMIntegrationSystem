using Common.Api.ProxyClient.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Timereg.Api.Repositories.Context;
using Timereg.Api.Repositories.ContextFactory;
using Timereg.Api.Repositories.Repositories;
using Timereg.Api.Services.Services;
using Integrations.Api.ProxyClient.Extensions;
using Shared.Interfaces;
using Shared.Services.Services;
using Timereg.Api.Unimicro.Adapters;
using Timereg.Api.Unimicro.HttpClients;

namespace Timereg.Api.Extensions
{
    public static class IoCExtension
    {
        public static void AddIocMapping(this IServiceCollection services)
        {
            services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddScoped<ITimeRegService, TimeRegService>();
            
            services.AddScoped<IExternalSystemFactory, ExternalSystemFactory>();
            services.AddScoped<UniMicroExternalDataService, UniMicroExternalDataService>();
            services.AddScoped<UniMicroAdapter, UniMicroAdapter>();
            services.AddScoped<UniMicroMatchingService, UniMicroMatchingService>();
    
            services.AddScoped<IExternalSystemService, ExternalSystemService>();
            services.AddScoped<IAbsenceService, AbsenceService>();
            services.AddScoped<IAbsenceExportService, AbsenceExportService>();
            services.AddScoped<INotificationService, NotificationService>();    
            services.AddScoped<IExternalSystemValidatorFactory, ExternalSystemValidatorFactory>();
            
            services.AddTransient<IDbContextFactory<TimeregDbContext>, DbContextFactory>();

            services.AddScoped<IAbsenceExportRepository, AbsenceExportRepository>();
            services.AddScoped<IExternalSystemRepository, ExternalSystemRepository>();
        }

        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            //Unimicro client config
            services.AddHttpClient<IUnimicroClient, UnimicroClient>();
            // user proxy  http client
            services.AddCommonHttpClient(configuration);
            services.AddIntegrationsHttpClient(configuration);
        }
    }
}
