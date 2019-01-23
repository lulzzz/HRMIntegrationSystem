using Common.Api.Repositories.Context;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using Common.Api.Repositories.Legacy.Models;
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

namespace Common.Api.Tests.ControllerTests.Employees
{
    public abstract class EmployeeControllerTestsBase
    {
        private TestServer _testServer;
        private PersonalLegacyContext _personalLegacyDb;
        private PersonalCommonLegacyContext _personalCommonLegacyDb;
        protected int _currentUserId = 81730;
        protected int _customerId = 1;
        protected HttpClient _client;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            Action<IServiceCollection> actions = (sc) =>
            {
                sc.AddScoped<IDbContextFactory<PersonalLegacyContext>, InMemoryPersonalLegacyContextFactory>();
                sc.AddScoped<IDbContextFactory<PersonalCommonLegacyContext>, InMemoryPersonalCommonLegacyContextFactory>();
                sc.AddScoped<IDbContextFactory<SticosWidgetDbContext>, InMemorySticosWidgetDbContextFactory>();
                var usercontext = new StaticUserContext(new UserContext
                {
                    UserId = _currentUserId
                });
                sc.Remove<ICurrentUserContext>();
                sc.AddScoped<ICurrentUserContext>(i => usercontext);
            };

            _testServer = new TestServerBuilder()
                .WithPostConfigureCollection(actions)
                .WithCurrentUser(_currentUserId, _customerId, true)
                .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _currentUserId);

            var fakeCustomerIdService = A.Fake<ICustomerIdService>();
            A.CallTo(() => fakeCustomerIdService.GetCustomerIdNotNull()).Returns(_customerId);

            var personalLegacyFactory = new InMemoryPersonalLegacyContextFactory(fakeCustomerIdService);
            var personalCommonLegacyFactory = _testServer.Host.Services.GetService<IDbContextFactory<PersonalCommonLegacyContext>>();

            _personalLegacyDb = await personalLegacyFactory.CreateDbContext();
            _personalCommonLegacyDb = await personalCommonLegacyFactory.CreateDbContext();
        }

        protected void AddCurrentUserToDb()
        {
            _personalCommonLegacyDb.Users.Add(new User { UserId = _currentUserId, IsPersonalCustomerAdmin = true, CustomerId = _customerId });
            _personalCommonLegacyDb.SaveChanges();
        }

        [TearDown]
        public async Task TearDownAfterEachTest()
        {
            _personalLegacyDb.Database.EnsureDeleted();
            _personalCommonLegacyDb.Database.EnsureDeleted();
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
            _personalCommonLegacyDb.Dispose();
            _client.Dispose();
            _testServer.Dispose();
        }

        protected async Task AddToPersonalCommonDb(List<Repositories.Legacy.Models.User> users)
        {
            _personalCommonLegacyDb.Users.AddRange(users);
            _personalCommonLegacyDb.SaveChanges();
        }

        protected async Task AddToPersonalDb(List<Repositories.Legacy.Models.Employee> employees)
        {
            _personalLegacyDb.Employees.AddRange(employees);
            _personalLegacyDb.SaveChanges();
        }

        protected async Task AddToPersonalDb(Repositories.Legacy.Models.Employee employee)
        {
            _personalLegacyDb.Employees.Add(employee);
            _personalLegacyDb.SaveChanges();
        }

        protected async Task AddPersonalAdminPermissionForUsers()
        {
            var employeePermission = new EmployeePermission
            {
                ResponsibleForUserId = _currentUserId,
                ResponsibleUserId = _currentUserId,
                PermissionType = 2,
                IsExplicit = true
            };
            _personalLegacyDb.EmployeePermissions.Add(employeePermission);
            _personalLegacyDb.SaveChanges();
        }
    }
}