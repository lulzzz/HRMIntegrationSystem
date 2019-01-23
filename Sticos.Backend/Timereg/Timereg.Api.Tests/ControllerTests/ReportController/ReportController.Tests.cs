using FakeItEasy;
using Integrations.Api.Contracts.Services;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;
using Shared.Exceptions;
using Shared.Services.Extensions;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Timereg.Api.Contracts;

namespace Timereg.Api.Tests.ControllerTests.ReportController
{
    [TestFixture]
    public class ReportControllerTests : ReportSetup
    {
        [Test]
        public async Task GetHourBalanceNotAuthorized()
        {
            var hourBalance = await _client.GetAsync("reports?unitId=1&employeeid=12345");

            Assert.NotNull(hourBalance);
            Assert.AreEqual(HttpStatusCode.NotFound, hourBalance.StatusCode);
        }

        [Test]
        public async Task GetHourBalanceEmployeeIdMissing()
        {
            var unitId = new Random().Next();
            var employeeId = new Random().Next();
            var integrationId = AddIntegrationServiceReturnsInvalid(unitId);
            AddValidEntityMapEmployee(unitId, integrationId, employeeId);
            var hourBalancResult = await _client.GetAsync($"{_customerId}/reports?unitId={unitId}");
            Assert.NotNull(hourBalancResult);
            Assert.AreEqual(HttpStatusCode.Forbidden, hourBalancResult.StatusCode);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _testServer.Dispose();
            _client.Dispose();
        }
    }
}
