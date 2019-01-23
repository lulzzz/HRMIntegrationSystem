using FakeItEasy;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Exceptions;
using Shared.Services;
using Shared.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timereg.Api.Controllers;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Unimicro.Validators;
using Timereg.Api.UnitTests.Builders;
using Timereg.Api.UnitTests.Helpers;
using contracts = Timereg.Api.Contracts;
using domain = Timereg.Api.Domain.Models;

namespace Timereg.Api.UnitTests
{
    [Ignore("refactor and/or qualitycheck test before readded")]
    [TestFixture]
    public class ReportUnitTests : BaseUnitTests
    {
        private IIntegrationService _integrationService => ServicesProvider.GetService<IIntegrationService>();
        private IEntityMapService _entityMapService => ServicesProvider.GetService<IEntityMapService>();

        [Test]
        [TestCase(600, 0, 10)]
        [TestCase(1200, 0, 20)]
        [TestCase(1800, 600, 20)]
        [TestCase(2400, 1800, 10)]
        public async Task TestReportResultOk(int actualMinutes, int expectedMinutes, int result)
        {
            var unimicroAdapter = A.Fake<IExternalSystemAdapter>();
            A.CallTo(() => unimicroAdapter.GetHourBalance(A<int>.Ignored, A<int>.Ignored))
                .Returns(new domain.HourBalance { ActualMinutes = actualMinutes, ExpectedMinutes = expectedMinutes });

            var externalSystemFactory = A.Fake<IExternalSystemFactory>();
            A.CallTo(() => externalSystemFactory.CreateSystemAdapter(A<domain.ExternalEconomySystem>.Ignored))
                .Returns(unimicroAdapter);


            var timeRegService = new TimeRegServiceBuilder()
                .WithExternalSystemFactory(externalSystemFactory)
                .WithIntegrationService(_integrationService)
                .WithCurrentUserContext(new StaticUserContext(new UserContext { UserId = 18 }))
                .Build();

            var Controller = new ReportController(timeRegService, Mapper);

            // act
            var response = await Controller.GetHourBalance(1, 1337);
            var okObjectResult = response.Result as OkObjectResult;
            contracts.HourBalance hourBalance = (contracts.HourBalance)okObjectResult.Value;

            // Assert
            Assert.NotNull(Controller);
            Assert.NotNull(hourBalance);
            Assert.AreEqual(result, hourBalance.HourBalanceInHours);
            Assert.IsInstanceOf(typeof(contracts.HourBalance), hourBalance);
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(1, 0)]
        [TestCase(null, null)]
        [TestCase(5, null)]
        [TestCase(10, 10)]
        public async Task TestReportIntegrationDoesNotExist(int userId, int unitId)
        {
            var currentUserContext = new StaticUserContext(new UserContext { UserId = userId });
            var timeRegService = new TimeRegServiceBuilder()
                .WithCurrentUserContext(currentUserContext)
                .WithIntegrationService(_integrationService)
                .WithValidator(new UnimicroValidator(_integrationService, _entityMapService))
                .Build();

            ReportController Controller = new ReportController(timeRegService, Mapper);

            // Assert
            Assert.NotNull(Controller);
            var thrownException = Assert.ThrowsAsync<ForbiddenException>(async () => await Controller.GetHourBalance(unitId, 1337));
            Assert.AreEqual("Company doesn't have integration", thrownException.Message);
        }

        [Test]
        [TestCase(0, 2)]
        [TestCase(1, 3)]
        public void TestReportIntegrationIsNotValid(int userId, int unitId)
        {
            var currentUserContext = new StaticUserContext(new UserContext { UserId = userId });
            var timeRegService = new TimeRegServiceBuilder()
                .WithCurrentUserContext(currentUserContext)
                .WithIntegrationService(_integrationService)
                .WithValidator(new UnimicroValidator(_integrationService, _entityMapService))
                .Build();

            ReportController Controller = new ReportController(timeRegService, Mapper);

            // Assert
            Assert.NotNull(Controller);
            var thrownException = Assert.ThrowsAsync<ForbiddenException>(async () => await Controller.GetHourBalance(unitId, 1337));
            Assert.AreEqual("Integration for company is not valid", thrownException.Message);
        }

        [Test]
        [TestCase(0, 4)]
        [TestCase(1, 5)]
        public void TestReportIntegrationDoesntHaveHourBalanceEnabled(int userId, int unitId)
        {
            var currentUserContext = new StaticUserContext(new UserContext { UserId = userId });
            var timeRegService = new TimeRegServiceBuilder()
                .WithCurrentUserContext(currentUserContext)
                .WithIntegrationService(_integrationService)
                .WithValidator(new UnimicroValidator(_integrationService, _entityMapService))
                .Build();

            ReportController Controller = new ReportController(timeRegService, Mapper);

            // Assert
            Assert.NotNull(Controller);
            var thrownException = Assert.ThrowsAsync<ForbiddenException>(async () => await Controller.GetHourBalance(unitId, 1337));
            Assert.AreEqual("Company doesn't support hour balance integration", thrownException.Message);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(4, 1)]
        [TestCase(5, 1)]
        [TestCase(6, 1)]
        [TestCase(7, 1)]
        [TestCase(8, 1)]
        public void TestReportUnimicroUserNotMappedException(int userId, int unitId)
        {
            var externalSystemFactory = ServicesProvider.GetService<IExternalSystemFactory>();

            var entityMapService = A.Fake<IEntityMapService>();
            A.CallTo(() => entityMapService.SearchEntityMaps(A<SearchQueryEntityMap>.Ignored))
                .Returns(new List<EntityMap>());
            var currentUserContext = new StaticUserContext(new UserContext { UserId = userId });
            var timeRegService = new TimeRegServiceBuilder()
                .WithExternalSystemFactory(externalSystemFactory)
                .WithCurrentUserContext(currentUserContext)
                .WithIntegrationService(_integrationService)
                .WithValidator(new UnimicroValidator(_integrationService, entityMapService))
                .Build();

            ReportController Controller = new ReportController(timeRegService, Mapper);

            // Assert
            Assert.NotNull(Controller);
            var thrownException = Assert.ThrowsAsync<ForbiddenException>(async () => await Controller.GetHourBalance(unitId, 1337));
            Assert.AreEqual("Employee is not mapped", thrownException.Message);
        }
    }
}
