using Absence.Api.Domain.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using sharedInterfaces = Shared.Domain.Interfaces;
using sharedDomain = Shared.Domain.Models;
using Shared.Services.Extensions;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Absence.Api.Tests.ControllerTests.StatisticsController
{
    public class StatisticsSetup
    {
        protected TestServer _testServer;
        protected HttpClient _client;
        private Action<IServiceCollection> _actions;
        private IStatisticsService _statisticsService;

        public static int CustomerId = 1;
        public int VacationChartId = 1;
        public int AbsenceChartId = 2;
        private readonly int _userId = 81730;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Common_Api_Url", "http://dummyUrl");

            _actions = (sc) =>
            {
                _statisticsService = A.Fake<IStatisticsService>();

                sc.ReplaceScoped<IStatisticsService>(_statisticsService);
            };

            _testServer = new TestServerBuilder()
                         .WithPostConfigureCollection(_actions)
                         .WithConfigSettings(settings)
                         .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(CustomerId, _userId);
        }

        public void AddVacationBankData()
        {
            var chartData = new sharedDomain.ChartData
            {
                Series = new List<sharedInterfaces.IChartSerie>
                    {
                        new sharedDomain.ChartSerie
                        {
                        },
                        new sharedDomain.ChartSerie
                        {
                        },
                        new sharedDomain.ChartSerie
                        {
                        },
                        new sharedDomain.ChartSerie
                        {
                        }
                    }
            };

            A.CallTo(() => _statisticsService.GetStatistics(A<int>.That.Matches(sq => sq == VacationChartId)))
                .Returns(chartData);
        }

        public void AddAbsenceData()
        {
            var chartData = new sharedDomain.ChartData
            {
                Series = new List<sharedInterfaces.IChartSerie>
                    {
                        new sharedDomain.ChartSerie
                        {
                        },
                        new sharedDomain.ChartSerie
                        {
                        },
                        new sharedDomain.ChartSerie
                        {
                        }
                    }
            };

            A.CallTo(() => _statisticsService.GetStatistics(A<int>.That.Matches(sq => sq == AbsenceChartId)))
                .Returns(chartData);
        }
        [OneTimeTearDown]
        public void TearDown()
        {
            _testServer.Dispose();
            _client.Dispose();
        }
    }
}
