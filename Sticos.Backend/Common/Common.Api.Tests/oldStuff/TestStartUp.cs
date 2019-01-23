using Common.Api.Repositories.Context;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Services;
using Shared.Services.Extensions;
using Shared.Services.Models;

namespace Common.Api.Tests
{
    public class CommonTestStartUp
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //override or replace services in ioc here
            services.ReplaceTransient<IDbContextFactory<PersonalLegacyContext>, InMemoryPersonalLegacyContextFactory>();
            services.ReplaceTransient<IDbContextFactory<PersonalCommonLegacyContext>, InMemoryPersonalCommonLegacyContextFactory>();
            services.ReplaceTransient<IDbContextFactory<SticosWidgetDbContext>, InMemorySticosWidgetDbContextFactory>();

            var usercontext = new StaticUserContext(new UserContext { UserId = 1 });
            services.Remove<ICurrentUserContext>();
            services.AddScoped<ICurrentUserContext>(i => usercontext);
        }
    }
}