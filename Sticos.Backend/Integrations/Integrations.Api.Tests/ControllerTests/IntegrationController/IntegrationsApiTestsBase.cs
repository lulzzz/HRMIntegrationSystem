using Common.Api.Contracts;
using Common.Api.Contracts.Services;
using Common.Api.Contracts.Users;
using FakeItEasy;
using Integrations.Api.Repositories.Context;
using Integrations.Api.Repositories.ContextFactory;
using MassTransit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Integrations.Api.Tests.ControllerTests.IntegrationController
{
    [TestFixture]
    public abstract class IntegrationsApiTestsBase
    {
        private TestServer _testServer;
        protected HttpClient _client;
        protected IntegrationDbContext _db;
        protected Action<IServiceCollection> _actions;

        protected readonly int _customerId = 1;
        protected readonly int _userId = 81730;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Common_Api_Url", "http://localhost");
            _actions = (sc) =>
            {
                sc.ReplaceScoped<IDbContextFactory<IntegrationDbContext>, InMemoryDbContextFactory>();

                var fakePublishEndpoint = A.Fake<IPublishEndpoint>();
                sc.AddSingleton(fakePublishEndpoint);

                var unitService = A.Fake<IUnitService>();
                A.CallTo(() => unitService.GetUnit(A<int>.Ignored))
                    .Returns(new Unit() { Id = 6, Name = "SticosAS", BusinessOrganizationNumber = "123456" });
                sc.Remove<IUnitService>();
                sc.AddSingleton<IUnitService>(unitService);
                var userService = A.Fake<IUserService>();
                A.CallTo(() => userService.GetUser(_userId)).Returns(new User() { IsPersonalCustomerAdmin = true, CustomerId = _customerId, UserId = _userId });
                sc.ReplaceScoped(userService);
            };

            _testServer = new TestServerBuilder()
                .WithPostConfigureCollection(_actions)
                .WithConfigSettings(settings)
                .Build<Startup>();
            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);

            var dbFactory = _testServer.Host.Services.GetService<IDbContextFactory<IntegrationDbContext>>();
            _db = await dbFactory.CreateDbContext();
        }
        protected async Task AddTodb(List<Repositories.Models.Integration> integrations)
        {
            _db.Integrations.AddRange(integrations);
            _db.SaveChanges();
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            _db.Database.EnsureDeleted();
            _testServer.Dispose();
            _client.Dispose();
        }
    }
}
