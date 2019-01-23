using Altinn.Api.Repositories.Context;
using Altinn.Api.Repositories.ContextFactory;
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

namespace Altinn.Api.Tests.ControllerTests.NavMessageController
{
    [TestFixture]
    public abstract class NavMessageControllerTestsBase
    {
        private TestServer _testServer;
        protected HttpClient _client;
        private AltinnDbContext _altinnDb;

        protected int _customerId = 1;
        private readonly int _userId = 81730;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Common_Api_Url", "http://localhost");
            Action<IServiceCollection> actions = (sc) =>
            {
                sc.ReplaceTransient<IDbContextFactory<AltinnDbContext>, InMemoryDbContextFactory>();
            };

            _testServer = new TestServerBuilder()
                .WithPostConfigureCollection(actions)
                .WithConfigSettings(settings)
                .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);

            var dbFactory = _testServer.Host.Services.GetService<IDbContextFactory<AltinnDbContext>>();
            _altinnDb = await dbFactory.CreateDbContext();
        }

        protected async Task AddToAltinnDb(Repositories.Models.NavMessage navMessage)
        {
            await _altinnDb.NavMessages.AddAsync(navMessage);
            await _altinnDb.SaveChangesAsync();
        }

        [TearDown]
        public async Task TearDownAfterEachTest()
        {
            _altinnDb.Database.EnsureDeleted();
        }

        [OneTimeTearDown]
        public async Task TearDownOneTime()
        {
            _altinnDb.Database.EnsureDeleted();
            _client.Dispose();
            _testServer.Dispose();
        }
    }
}
