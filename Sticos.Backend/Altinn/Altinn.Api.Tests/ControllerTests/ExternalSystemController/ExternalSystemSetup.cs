using Altinn.Api.Client.Constants;
using Altinn.Api.Client.HttpClients;
using Altinn.Api.Client.Models;
using Altinn.Api.Repositories.Context;
using Altinn.Api.Services;
using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Contracts;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Altinn.Api.Tests.ControllerTests.ExternalSystemController
{
    public class ExternalSystemSetup
    {
        protected TestServer _testServer;
        protected HttpClient _client;
        private IMapper _mapper;
        private Action<IServiceCollection> _actions;
        protected readonly int _customerId = 1;
        private readonly int _userId = 81730;
        private AltinnExternalDataService _altinnExternalDataService;
        private IAltinnClient _altinnClient;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Common_Api_Url", "http://localhost");
            _actions = (sc) =>
            {
                _altinnClient = A.Fake<IAltinnClient>();

                sc.ReplaceScoped<IAltinnClient>(_altinnClient);
            };

            _testServer = new TestServerBuilder()
                        .WithPostConfigureCollection(_actions)
                        .WithConfigSettings(settings)
                        .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);

        }

        public void PopulateExternalData(int numberOfExternalEntities)
        {
            var externalDataFake = A.CollectionOfFake<Reportee>(numberOfExternalEntities).ToList();

            A.CallTo(() => _altinnClient.GetReportees())
                .Returns(externalDataFake);
        }
    }
}