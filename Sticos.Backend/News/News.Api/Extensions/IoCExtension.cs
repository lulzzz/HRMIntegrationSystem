using Common.Api.ProxyClient.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using News.Api.Services;
using Shared.Interfaces;
using Shared.Services.Services;

namespace News.Api.Extensions
{

    public static class IoCExtension
    {
        public static void AddIocMapping(this IServiceCollection services)
        {
            services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();

            services.AddScoped<IEntityFilterService<Models.News, int>, NewsFilterService>();
            services.AddScoped<IEntityFilterService<Models.NewsAttachment, int>, NewsAttachmentFilterService>();
            services.AddScoped<IEntityAuthorizationService<Models.News, int>, NewsAuthorizationService>();
            services.AddScoped<IEntityAuthorizationService<Models.NewsAttachment, int>, NewsAttachmentAuthorizationService>();
        }

        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCommonHttpClient(configuration);
        }
    }
}
