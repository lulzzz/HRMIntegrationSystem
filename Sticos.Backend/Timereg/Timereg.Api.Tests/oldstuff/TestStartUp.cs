using System;
using System.Collections.Generic;
using FakeItEasy;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Timereg.Api.Repositories.Context;
using Timereg.Api.Repositories.ContextFactory;

namespace Timereg.Api.UnitTests
{
    public class TimeRegTestStartUp //: Startup
    {
        public TimeRegTestStartUp(IHostingEnvironment env) //: base(env)
        {
            
        }

        public Action<IServiceCollection> ConfigureAction { get; set; }


        public  IServiceCollection ConfigureServices(IServiceCollection services)
        {
           

            //override or replace services in ioc here
            services.ReplaceScoped<IDbContextFactory<TimeregDbContext>, InMemoryDbContextFactory>();

            var integrationService = A.Fake<IIntegrationService>();

            A.CallTo(() => integrationService.Search(A<SearchQueryIntegration>.Ignored))
                .Returns(new List<Integrations.Api.Contracts.Integration>
                {
                    new Integrations.Api.Contracts.Integration
                    {
                        Id = 1,
                        Category = 1,
                        IsActivated = true,
                        ExternalSystem = 2,
                        UnitId = 1,
                    }
                });

            services.Remove<IIntegrationService>();
            services.AddSingleton(integrationService);

            return services;
        }
    }
}