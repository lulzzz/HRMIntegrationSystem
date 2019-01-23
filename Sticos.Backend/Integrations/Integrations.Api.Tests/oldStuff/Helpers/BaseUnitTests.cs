using Microsoft.Extensions.DependencyInjection;
using Integrations.Api.Extensions;
using Integrations.Api.Repositories.ContextFactory;
using Integrations.Api.Repositories.Context;
using Shared.Interfaces;
using Shared.Services.Extensions;


namespace Integrations.Api.UnitTests.Helpers
{
    public abstract class BaseUnitTests
    {
        protected ServiceProvider ServicesProvider;

        protected AutoMapper.IMapper Mapper => ServicesProvider.GetService<AutoMapper.IMapper>();

        public BaseUnitTests()
        {
        }

        public void OneTimeSetUp()
        {
            ServicesProvider = GetServices().BuildServiceProvider();
        }

        public ServiceCollection GetServices()
        {
            var services = new ServiceCollection();
            services.AddIocMapping();
            services.AddAutoMapper(AutoMapperSetup.Config);

            services.ReplaceScoped<IDbContextFactory<IntegrationDbContext>, InMemoryDbContextFactory>();
            return services;
        }

    }
}
