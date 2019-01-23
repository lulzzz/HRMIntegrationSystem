using Common.Api.ProxyClient.Extensions;
using Integration.Api.Repositories.Repositories;
using Integrations.Api.Domain.Interfaces;
using Integrations.Api.Domain.Services;
using Integrations.Api.Domain.Validators;
using Integrations.Api.Domain.Validators.Interfaces;
using Integrations.Api.MessageBus;
using Integrations.Api.Repositories.Context;
using Integrations.Api.Repositories.ContextFactory;
using Integrations.Api.Repositories.Repositories;
using Integrations.Api.Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.MessageBus.Contracts;
using Shared.Services.Services;
using domain = Integrations.Api.Domain.Models;

namespace Integrations.Api.Extensions
{
    public static class IoCExtension
    {
        public static void AddIocMapping(this IServiceCollection services)
        {
            services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddScoped<IIntegrationService, IntegrationService>();
            services.AddScoped<IIntegrationCategoryService, IntegrationCategoryService>();
            services.AddScoped<IEntityMapService, EntityMapService>();

            services.AddScoped<IIntegrationCategoryRepository, IntegrationCategoryRepository>();
            services.AddScoped<IEntityValidator<domain.Integration, int?>, IntegratorValidator>();
            services.AddScoped<IEntityMapRepository, EntityMapRepository>();

            services.AddTransient<IDbContextFactory<IntegrationDbContext>, DbContextFactory>();

            services.AddScoped<IRepository<domain.Integration, domain.SearchQueryIntegration>, IntegrationRepository>();
            services.AddScoped<IPublisher<IIntegrationDeleted>, IntegrationDeletePublisher>();
        }

        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            // user proxy  http client
            services.AddCommonHttpClient(configuration);
        }
    }
}
