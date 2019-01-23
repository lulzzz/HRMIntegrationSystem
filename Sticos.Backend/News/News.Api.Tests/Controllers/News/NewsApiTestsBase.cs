using Common.Api.Contracts.Users;
using FakeItEasy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using News.Api.Repository;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.Services;
using Shared.Services.Extensions;
using Shared.Services.Models;
using Shared.Services.Services;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace News.Api.Tests.Controllers.NewsController
{
    [TestFixture]
    public abstract class NewsApiTestsBase
    {
        private TestServer _testServer;
        protected HttpClient _client;
        protected NewsContext _db;
        protected Action<IServiceCollection> _actions;

        protected readonly int _customerId = 1;
        protected readonly int _userId = 53963;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Common_Api_Url", "http://localhost");
            _actions = (sc) =>
            {
                sc.ReplaceScoped<IDbContextFactory<NewsContext>, InMemoryContextFactory<NewsContext>>();

                var usercontext = new StaticUserContext(new UserContext { UserId = _userId });
                sc.Remove<ICurrentUserContext>();
                sc.AddScoped<ICurrentUserContext>(i => usercontext);

                var userService = A.Fake<IUserService>();
                A.CallTo(() => userService.GetUser(_userId)).Returns(new User() { IsPersonalCustomerAdmin = true, CustomerId = _customerId, UserId = _userId });
                sc.ReplaceScoped(userService);
            };

            _testServer = new TestServerBuilder()
                .WithPostConfigureCollection(_actions)
                .WithConfigSettings(settings)
                .WithCurrentUser(_userId, _customerId, true)
                .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);

            var fakeCustomerIdService = A.Fake<ICustomerIdService>();
            A.CallTo(() => fakeCustomerIdService.GetCustomerIdNotNull()).Returns(_customerId);

            var dbFactory = _testServer.Host.Services.GetService<IDbContextFactory<NewsContext>>();
            _db = await dbFactory.CreateDbContext();
        }

        protected async Task AddTodb(IEnumerable<Models.News> items)
        {
            _db.Set<Models.News>().AddRange(items);
            await _db.SaveChangesAsync();
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
