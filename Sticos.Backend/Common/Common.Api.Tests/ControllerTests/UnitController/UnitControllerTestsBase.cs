using Common.Api.Repositories.Context;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using FakeItEasy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.Services;
using Shared.Services.Extensions;
using Shared.Services.Models;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Api.Tests.ControllerTests
{
    public abstract class UnitControllerTestsBase
    {
        private TestServer _testServer;
        private PersonalLegacyContext _personalLegacyDb;

        protected HttpClient _client;

        protected int _customerId = 1;
        private readonly int _userId = 81730;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var fakeCustomerIdService = A.Fake<ICustomerIdService>();
            A.CallTo(() => fakeCustomerIdService.GetCustomerIdNotNull()).Returns(_customerId);
            Action<IServiceCollection> actions = (sc) =>
            {
                sc.ReplaceScoped<ICustomerIdService>(fakeCustomerIdService);
                sc.ReplaceTransient<IDbContextFactory<PersonalLegacyContext>, InMemoryPersonalLegacyContextFactory>();
                sc.ReplaceTransient<IDbContextFactory<SticosWidgetDbContext>, InMemorySticosWidgetDbContextFactory>();
                var usercontext = new StaticUserContext(new UserContext { UserId = _userId });
                sc.Remove<ICurrentUserContext>();
                sc.AddScoped<ICurrentUserContext>(i => usercontext);
            };

            _testServer = new TestServerBuilder()
                .WithPostConfigureCollection(actions)
                .WithCurrentUser(1, _customerId, true)
                .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);

            var personalLegacyFactory = _testServer.Host.Services.GetService<IDbContextFactory<PersonalLegacyContext>>();
            _personalLegacyDb = await personalLegacyFactory.CreateDbContext();
        }

        [TearDown]
        public async Task TearDownAfterEachTest()
        {
            _personalLegacyDb.Database.EnsureDeleted();
        }

        [OneTimeTearDown]
        public async Task TearDownOneTime()
        {

            _client.Dispose();
            _testServer.Dispose();
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            _personalLegacyDb.Dispose();
            _client.Dispose();
            _testServer.Dispose();
        }

        protected async Task AddToPersonalDb(List<Repositories.Legacy.Models.Unit> units)
        {
            await _personalLegacyDb.Units.AddRangeAsync(units);
            await _personalLegacyDb.SaveChangesAsync();
        }

        protected async Task AddToPersonalDb(Repositories.Legacy.Models.OrgUnitVerification orgUnitVerification)
        {
            _personalLegacyDb.OrgUnitVerifications.Add(orgUnitVerification);
            _personalLegacyDb.SaveChanges();
        }
    }
}