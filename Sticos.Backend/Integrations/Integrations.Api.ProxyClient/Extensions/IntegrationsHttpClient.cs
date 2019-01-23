using Integrations.Api.Contracts.Services;
using Integrations.Api.ProxyClient.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Shared.Services;
using Shared.Services.Extensions;

namespace Integrations.Api.ProxyClient.Extensions
{
    public static class IntegrationsHttpClient
    {
        public static void AddIntegrationsHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValueNotNull<string>(Constants.API_URL_CONFIG_KEY);
            services.AddHttpClient(Constants.API_CLIENT_NAME, client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddScoped<IIntegrationService, IntegrationsProxyClient>();
            services.AddScoped<IEntityMapService, IntegrationsProxyClient>();
        }
    }
}
