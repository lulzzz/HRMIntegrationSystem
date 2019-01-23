using Absence.Api.Tests.ControllerTests.StatisticsController.Models;
using NUnit.Framework;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Absence.Api.Tests.ControllerTests.StatisticsController
{
    [TestFixture]
    public class StatisticsGetStatistics : StatisticsSetup
    {
        [Test]
        public async Task WhenQueryStatisticsWithUnknownCustomerId_ThenForbiddenShouldBeReturned()
        {
            var unknownCustomerId = 1231412;
            var randomNumber = new Random().Next(10000, 10000000);
            var statistics = await _client.GetAsync($"{unknownCustomerId}/statistics/{randomNumber}");

            Assert.NotNull(statistics);
            Assert.AreEqual(HttpStatusCode.Forbidden, statistics.StatusCode);
        }

        [Test]
        public async Task QueryForVacationBankWidgetData()
        {
            var numberOfSeries = 4;
            AddVacationBankData();

            var statistics = await _client.GetAsyncAndDeserialize<ChartData>($"{CustomerId}/statistics/{VacationChartId}");
            var series = statistics.Series;

            Assert.NotNull(statistics);
            Assert.AreEqual(numberOfSeries, series.Count);
        }

        [Test]
        public async Task QueryForAbsenceWidgetData()
        {
            var numberOfSeries = 3;
            AddAbsenceData();

            var statistics = await _client.GetAsyncAndDeserialize<ChartData>($"{CustomerId}/statistics/{AbsenceChartId}");
            var series = statistics.Series;

            Assert.NotNull(statistics);
            Assert.AreEqual(numberOfSeries, series.Count);
        }
    }
}
