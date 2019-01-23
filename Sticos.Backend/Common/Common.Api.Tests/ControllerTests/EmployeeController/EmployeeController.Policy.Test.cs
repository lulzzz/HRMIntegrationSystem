using Common.Api.Contracts.Users;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using FakeItEasy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Shared.TestCommon;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Common.Api.Tests.ControllerTests.Employees
{
    [TestFixture]
    public class EmployeeControllerPolicyTest
    {
        private TestServer _testServer;

        protected HttpClient _client;
        protected int _customerId = 1;
        private readonly int _userId = 81730;

        [OneTimeSetUp]
        public void SetUp()
        {
            Action<IServiceCollection> actions = (sc) =>
            {
                var userServiceFake = A.Fake<IUserService>();
                A.CallTo(() => userServiceFake.GetUser(1)).Returns(Task.FromResult<IUser>(new User
                {
                    CustomerId = _customerId,
                    IsPersonalCustomerAdmin = false,
                    UserId = 1
                }));
                sc.ReplaceSingleton(userServiceFake);
                sc.ReplaceTransient<IDbContextFactory<PersonalCommonLegacyContext>, InMemoryPersonalCommonLegacyContextFactory>();
                sc.ReplaceTransient<IDbContextFactory<PersonalLegacyContext>, InMemoryPersonalLegacyContextFactory>();
            };

            _testServer = new TestServerBuilder()
                .WithPostConfigureCollection(actions)
                .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);
        }

        [Test]
        public async Task RequestEmployeesUserNotAdmin_403Forbidden()
        {
            var url = $"{_customerId}/employees";

            var response = await _client.GetAsync(url);

            Assert.AreEqual(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
