using Common.Api.Contracts;
using Common.Api.Contracts.Services;
using FakeItEasy;
using Integrations.Api.Repositories.Context;
using Integrations.Api.Repositories.ContextFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Shared.Interfaces;
using Shared.Services.Extensions;


namespace Integrations.Api.UnitTests
{
    public class IntegrationsTestStartUp
    {
        public IntegrationsTestStartUp(IConfiguration configuration)
        {
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ReplaceScoped<IDbContextFactory<IntegrationDbContext>, InMemoryDbContextFactory>();

            var unitService = A.Fake<IUnitService>();
            A.CallTo(() => unitService.GetUnit(A<int>.Ignored))
                     .Returns(new Unit() { Id = 6, Name = "SticosAS", BusinessOrganizationNumber = "123456" });
            services.Remove<IUnitService>();
            services.AddSingleton<IUnitService>(unitService);
        }
    }
}
