using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using Shared.Contracts;
using Shared.TestCommon;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Api.Tests.ControllerTests.AbsenceType
{
    [TestFixture]
    public class AbsenceTypesControllerTests
    {
        private TestServer _testServer;
        protected HttpClient _client;
        protected int _customerId = 1;
        private readonly int _userId = 81730;

        [OneTimeSetUp]
        public void SetUp()
        {
            _testServer = new TestServerBuilder()
                .Build<Startup>();
            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);
        }

        [Test]
        public async Task TestGetAbsenceTypesOkStatus()
        {
            // Act
            var absenceTypes = await _client.GetAsyncAndDeserialize<List<Code>>($"{_customerId}/absencestypes");

            // Assert
            Assert.NotNull(absenceTypes);
            Assert.IsTrue(absenceTypes.Count >= 19);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testServer.Dispose();
            _client.Dispose();
        }
    }
}
