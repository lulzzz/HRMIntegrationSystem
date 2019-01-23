using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Timereg.Api.Extensions;
using Timereg.Api.Unimicro.Constants;
using Timereg.Api.Unimicro.HttpClients;

namespace Timereg.Api.IntegrationTests.AdapterTests.Unimicro
{
    public abstract class UnimicroTestsBase
    {
        protected string _companyKey = "ccdf32a2-ed01-452a-b7a5-df3b90a1a7b7";
        protected string _legalOrganizationNumber = "934228391";
        protected string _businessOrganizationNumber = "971998016";

        protected IUnimicroClient _proxy;
        private ServiceProvider _provider;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var configSettings = new Dictionary<string, string>();
            configSettings.Add(Constants.API.API_PASSWORD_CONFIG_KEY, "Sticos-integration");
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddIocMapping();
            serviceCollection.AddHttpClient<IUnimicroClient, UnimicroClient>();
            serviceCollection.AddScoped<IConfiguration>(c => config);
            _provider = serviceCollection.BuildServiceProvider();

            _proxy = _provider.GetService<IUnimicroClient>();

              var result = _proxy.SignIn().Result;
              var company = _proxy.GetAndSetCompanyAuthorizationInfo(_legalOrganizationNumber).Result;
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _provider?.Dispose();
        }


    }
}