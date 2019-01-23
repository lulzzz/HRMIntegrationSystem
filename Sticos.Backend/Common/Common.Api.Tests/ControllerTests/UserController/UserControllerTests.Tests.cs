using Common.Api.Contracts.Users;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.Services;
using Shared.Services.Extensions;
using Shared.Services.Models;
using Shared.TestCommon;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Api.Tests.ControllerTests.UserController
{
    [TestFixture]
    public class UserControllerTests
    {
        private TestServer _testServer;
        protected HttpClient _client;
        private PersonalCommonLegacyContext _personalCommonLegacyDb;

        private readonly int _userId = 81730;
        private const int _customerId = 1;

        [OneTimeSetUp]
        public async Task SetUp()
        {

            _testServer = new TestServerBuilder()
                .WithPostConfigureCollection((sc) =>
                {
                    sc.ReplaceTransient<IDbContextFactory<PersonalCommonLegacyContext>, InMemoryPersonalCommonLegacyContextFactory>();
                    var usercontext = new StaticUserContext(new UserContext { UserId = _userId });
                    sc.Remove<ICurrentUserContext>();
                    sc.AddScoped<ICurrentUserContext>(i => usercontext);
                })
                .Build<Startup>();
            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);

            var personalCommonLegacyFactory = _testServer.Host.Services.GetService<IDbContextFactory<PersonalCommonLegacyContext>>();
            _personalCommonLegacyDb = await personalCommonLegacyFactory.CreateDbContext();
        }

        [Test]
        public async Task TestGetCurrentUserOkStatus()
        {
            //Arrange
            _personalCommonLegacyDb.Users.Add(new Repositories.Legacy.Models.User() { UserId = _userId });
            _personalCommonLegacyDb.SaveChanges();

            // Act
            var user = await _client.GetAsyncAndDeserialize<User>($"{_customerId}/users/{_userId}");

            // Assert
            Assert.NotNull(user);
            Assert.AreEqual(_userId, user.UserId);
        }


        [TearDown]
        public async Task TearDownAfterEachTest()
        {
            _personalCommonLegacyDb.Database.EnsureDeleted();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testServer.Dispose();
            _client.Dispose();
        }
    }
}
