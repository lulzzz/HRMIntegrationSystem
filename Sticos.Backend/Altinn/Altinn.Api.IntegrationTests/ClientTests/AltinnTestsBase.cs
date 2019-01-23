using System.IO;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

using Altinn.Api.Client.Constants;
using Altinn.Api.Client.HttpClients;

namespace Altinn.Api.IntegrationTests.ClientTests
{
    public abstract class AltinnTestsBase
    {
        public IAltinnClient _altinnClient;
        private ServiceProvider _provider;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var cerfiticate = GetAltinnCertificate(config);
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddHttpClient<IAltinnClient, AltinnClient>()
            .ConfigurePrimaryHttpMessageHandler(() =>
             {
                 var handler = new HttpClientHandler();
                 handler.ClientCertificates.Add(cerfiticate);
                 return handler;
             });

            serviceCollection.AddScoped(c => config);
            _provider = serviceCollection.BuildServiceProvider();

            _altinnClient = _provider.GetService<IAltinnClient>();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _provider?.Dispose();
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
