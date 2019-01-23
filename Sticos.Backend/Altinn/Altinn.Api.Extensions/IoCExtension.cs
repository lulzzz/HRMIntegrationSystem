using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Shared.Interfaces;
using Shared.Services;
using Shared.Services.Models;
using Common.Api.ProxyClient.Extensions;

using Altinn.Api.Client.Serializers;
using Altinn.Api.Domain.Interfaces;
using Altinn.Api.Domain.Entities;
using Altinn.Api.Services;
using Altinn.Api.Repositories;
using Altinn.Api.Repositories.ContextFactory;
using Altinn.Api.Repositories.Context;
using Altinn.Api.Repositories.Repositories;
using Altinn.Api.Client.HttpClients;
using Altinn.Api.Client.Adapters;
using Altinn.Api.Client.Constants;

namespace Altinn.Api.Extensions
{
    public static class IoCExtension
    {
        public static void AddIocMapping(this IServiceCollection services)
        {
            var userContext = new StaticUserContext(new UserContext());
            services.AddSingleton<ICurrentUserContext>(userContext);

            services.AddScoped<IAltinnAdapter, AltinnAdapter>();
            services.AddScoped<INavMessageService, NavMessageService>();
            services.AddScoped<IExternalSystemService, ExternalSystemService>();
            services.AddScoped<IXmlSerializer, NavMessageXmlSerializer>();
            services.AddScoped<IRepository<NavMessage, SearchQueryNavMessage>, NavMessageRepository>();
            services.AddScoped<IRepository<ExternalSystem>, ExternalSystemRepository>();
            services.AddScoped<AltinnExternalDataService, AltinnExternalDataService>();
            services.AddScoped<IExternalSystemFactory, ExternalSystemFactory>();
            services.AddTransient<IDbContextFactory<AltinnDbContext>, DbContextFactory>();
        }

        public static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var cerfiticate = GetAltinnCertificate(configuration);

            services.AddHttpClient<IAltinnClient, AltinnClient>()
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(cerfiticate);
                return handler;
            });

            // Use proxy client
            services.AddCommonHttpClient(configuration);
        }

        private static X509Certificate2 GetAltinnCertificate(IConfiguration configuration)
        {
            var basePath = Directory.GetCurrentDirectory();
            var certificateLocation = configuration.GetSection("Altinn.External.Api").GetValue<string>(Constants.API.API_CERTIFICATE_PATH_CONFIG_KEY);
            var certificatePath = Path.Combine(basePath, certificateLocation);

            var fileExists = File.Exists(certificatePath);
            if (!fileExists)
                throw new ArgumentException(certificatePath);

            var certificatePassword = configuration.GetSection("Altinn.External.Api").GetValue<string>(Constants.API.API_CERTIFICATE_PASSWORD_CONFIG_KEY);
            var certificate = new X509Certificate2(certificatePath, certificatePassword);

            return certificate;
        }
    }
}