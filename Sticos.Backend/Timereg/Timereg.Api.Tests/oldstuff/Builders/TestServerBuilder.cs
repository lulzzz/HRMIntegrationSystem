using System;
using System.Collections.Generic;
using System.IO;
using FakeItEasy;
using Integrations.Api.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Timereg.Api.UnitTests.Builders
{
    //public class TestServerBuilder
    //{
    //    private string _environment = "Development";
    //    private string _contentRootPath = Directory.GetCurrentDirectory();
    //    private Action<IServiceCollection> _postConfigureServiceCollection = (sc) =>{};
    //    private Dictionary<string, string> _configSettings = new Dictionary<string, string>();

    //    public TestServerBuilder WithEnvironment(string environment)
    //    {
    //        _environment = environment;
    //        return this;
    //    }

    //    public TestServerBuilder WithPostConfigureCollection(Action<IServiceCollection> postConfigureServiceCollection)
    //    {
    //        _postConfigureServiceCollection = postConfigureServiceCollection;
    //        return this;
    //    } 
        
    //    public TestServerBuilder WithConfigSettings(Dictionary<string, string> configSettings)
    //    {
    //        _configSettings = configSettings;
    //        return this;
    //    }
    //    public TestServerBuilder WithContentRootPath(string contentRootPath)
    //    {
    //        _contentRootPath = contentRootPath;
    //        return this;
    //    }

    //    public TestServer Build<TStartUp>() where TStartUp : class, IConfigurableStartUp, new()
    //    {
    //        var hostingEnvironment = A.Fake<IHostingEnvironment>();
    //        A.CallTo(() => hostingEnvironment.EnvironmentName).Returns(_environment);
    //        A.CallTo(() => hostingEnvironment.ContentRootPath).Returns(_contentRootPath);
    //        var myStartup = (TStartUp) Activator.CreateInstance(typeof(TStartUp), hostingEnvironment);
        
    //        //var myStartup = new Startup(hostingEnvironment);
    //       // var integrationService = A.Fake<IIntegrationService>();
    //        //var serviceDescriptor = new ServiceDescriptor(typeof(IIntegrationService), integrationService);
    //        myStartup.PostConfigureServiceCollection = _postConfigureServiceCollection;
    //        //  myStartup.PostConfigureServiceCollection = (sc) => { sc.Replace(serviceDescriptor); };
            
    //        var existingConfig = new MemoryConfigurationSource
    //        {
    //            InitialData = myStartup.Configuration.AsEnumerable()
    //        };

    //        // create new configuration from existing config
    //        // and override whatever needed
    //        var testConfigBuilder = new ConfigurationBuilder()
    //            .Add(existingConfig)
    //            .AddInMemoryCollection(_configSettings);

    //        myStartup.Configuration = testConfigBuilder.Build();
            
    //        var builder = new WebHostBuilder()
    //                .ConfigureServices(services => 
    //                    { services.AddSingleton<IStartup>(myStartup); });

    //        return  new TestServer(builder);

    //    }

        
    //}
}