using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using System.Net.Http;
using FakeItEasy;
using Integrations.Api.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Timereg.Api.UnitTests
{
    [TestFixture]
    public abstract class TimeregApiTestsBase
    {
        private TestServer _testServer;
        protected HttpClient _client;

        [OneTimeSetUp]
        public void SetUp()
        {
            var hostingEnvironment = A.Fake<IHostingEnvironment>();
            A.CallTo(() => hostingEnvironment.EnvironmentName).Returns("Development");
            A.CallTo(() => hostingEnvironment.ContentRootPath).Returns(Directory.GetCurrentDirectory());

            var myStartup = new Startup(null, hostingEnvironment,null);
            var integrationService = A.Fake<IIntegrationService>();
            var serviceDescriptor = new ServiceDescriptor(typeof(IIntegrationService), integrationService);
            myStartup.PostConfigureServiceCollection = (sc) => { };
          //  myStartup.PostConfigureServiceCollection = (sc) => { sc.Replace(serviceDescriptor); };
            
            // create new configuration from existing config
            // and override whatever needed
            var testConfigBuilder = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>()
                {
                    { "DbContext:ConnectionString", "DataSource=:memory:" }
                });
            
            var builder = new WebHostBuilder()
                .ConfigureServices(services =>
                { 
                    //services.AddSingleton<IStartup>(myStartup);
                })
                ;

            _testServer = new TestServer(builder);


            //_testServer = new TestServerBuilder()
            //    //.WithRelativeDirectory("Timereg/Timereg.Api")
            //    .Build();
            //_client = _testServer.CreateClient();
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            _testServer.Dispose();
            _client.Dispose();
        }
    }
}
