using Timereg.Api.Extensions;
using Timereg.Api.Repositories.Context;
using Timereg.Api.Repositories.ContextFactory;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using FakeItEasy;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Integrations.Api.Contracts.Services;
using Shared.Interfaces;
using Shared.Services.Extensions;


namespace Timereg.Api.UnitTests.Helpers
{
   public abstract class BaseUnitTests
    {
        protected ServiceProvider ServicesProvider;

        protected AutoMapper.IMapper Mapper => ServicesProvider.GetService<AutoMapper.IMapper>();

        public BaseUnitTests()
        {
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ServicesProvider = GetServices().BuildServiceProvider();
        }

        public ServiceCollection GetServices()
        {
            var configuration = A.Fake<IConfiguration>();
            A.CallTo(() => configuration["Common_Api_Url"]).Returns("http://localhost:5000/");
            A.CallTo(() => configuration["Integrations_Api_Url"]).Returns("http://localhost:61256/");

            var services = new ServiceCollection();
            services.AddIocMapping();
            services.AddAutoMapper(AutoMapperSetup.Config);
            
            services.ReplaceScoped<IDbContextFactory<TimeregDbContext>, InMemoryDbContextFactory>();
          //  services.ReplaceScoped<IIntegrationService, InMemoryDbContextFactory>();
            services.AddHttpClients(configuration);
            return services;
        }
    }
}
