using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using Shared.TestCommon;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Api.Tests.ControllerTests.CurrentUserController
{
    [TestFixture]
    public class CurrentUserControllerTests
    {
        private TestServer _testServer;

        private HttpClient _client;
        private readonly int _customerId = 123345;
        private readonly int _userId = 81730;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            _testServer = new TestServerBuilder()
                .Build<Startup>();
            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);
        }

        [Test]
        public async Task GetCurrentUserReturnsTokenStuff()
        {
            var userString = await _client.GetStringAsync("/currentuser");

            Assert.AreEqual($"{{\"userId\":81730,\"customerId\":{_customerId}}}", userString);
        }
    }
}
