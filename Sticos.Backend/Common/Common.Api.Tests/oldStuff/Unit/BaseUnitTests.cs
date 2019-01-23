using AutoMapper;
using Common.Api.Extensions;
using Common.Api.Repositories.Context;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.Services;
using Shared.Services.Extensions;
using Shared.Services.Models;

namespace Common.Api.Tests.Unit
{
    public abstract class BaseUnitTests
    {
        protected const int DefaultTake = 100;
        protected ServiceProvider ServicesProvider;

        protected IMapper Mapper => ServicesProvider.GetService<IMapper>();

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ServicesProvider = GetServices().BuildServiceProvider();
        }

        public ServiceCollection GetServices()
        {
            var services = new ServiceCollection();

            services.AddAutoMapper(AutoMapperSetup.Config);

            services.ReplaceTransient<IDbContextFactory<SticosWidgetDbContext>, InMemorySticosWidgetDbContextFactory>();
            services.ReplaceTransient<IDbContextFactory<PersonalLegacyContext>, InMemoryPersonalLegacyContextFactory>();

            var userContext = new StaticUserContext(new UserContext { UserId = 1 });

            services.Remove<ICurrentUserContext>();
            services.AddScoped<ICurrentUserContext>(i => userContext);

            return services;
        }
    }
}