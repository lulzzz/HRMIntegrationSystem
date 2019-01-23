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

namespace Common.Api.Tests.ControllerTests.UserController
{
    public class UserControllerTestBase
    {
        private TestServer _testServer;
        private PersonalCommonLegacyContext _personalCommonLegacyDb;

        protected readonly int _userId = 81730;
        protected readonly int _customerId = 1;

        protected HttpClient _client;
        private PersonalLegacyContext _personalLegacyDb;

        [OneTimeSetUp]
        public async Task Setup()
        {
            Action<IServiceCollection> actions = (sc) =>
            {
                sc.ReplaceTransient<IDbContextFactory<PersonalCommonLegacyContext>, InMemoryPersonalCommonLegacyContextFactory>();
                sc.ReplaceTransient<IDbContextFactory<PersonalLegacyContext>, InMemoryPersonalLegacyContextFactory>();

                var usercontext = new StaticUserContext(new UserContext { UserId = _userId });
                sc.Remove<ICurrentUserContext>();
                sc.AddScoped<ICurrentUserContext>(i => usercontext);
            };

            _testServer = new TestServerBuilder()
                .WithPostConfigureCollection(actions)
                .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);

            var fakeCustomerIdService = A.Fake<ICustomerIdService>();
            A.CallTo(() => fakeCustomerIdService.GetCustomerIdNotNull()).Returns(_customerId);

            var personalLegacyFactory = new InMemoryPersonalLegacyContextFactory(fakeCustomerIdService);
            _personalLegacyDb = await personalLegacyFactory.CreateDbContext();

            var personalCommonLegacyFactory = _testServer.Host.Services.GetService<IDbContextFactory<PersonalCommonLegacyContext>>();
            _personalCommonLegacyDb = await personalCommonLegacyFactory.CreateDbContext();
        }

        [TearDown]
        public void TearDownAfterEachTest()
        {
            _personalCommonLegacyDb.Database.EnsureDeleted();
            _personalLegacyDb.Database.EnsureDeleted();
        }

        [OneTimeTearDown]
        public void TearDownOneTime()
        {
            _personalCommonLegacyDb.Dispose();
            _personalLegacyDb.Dispose();
            _client.Dispose();
            _testServer.Dispose();
        }

        protected void AddUsersToDb(List<Repositories.Legacy.Models.User> users)
        {
            _personalCommonLegacyDb.Users.AddRange(users);
            _personalCommonLegacyDb.SaveChanges();
        }
        protected void AddUserPermissions(List<Repositories.Legacy.Models.EmployeePermission> employeePermissions)
        {
            _personalLegacyDb.EmployeePermissions.AddRange(employeePermissions);
            _personalLegacyDb.SaveChanges();
        }
    }
}