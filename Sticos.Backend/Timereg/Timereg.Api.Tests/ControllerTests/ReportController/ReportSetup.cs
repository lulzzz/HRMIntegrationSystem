using AutoMapper;
using FakeItEasy;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Shared.TestCommon;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;
using Timereg.Api.Repositories.Context;
using Timereg.Api.Repositories.ContextFactory;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Tests.ControllerTests.ReportController
{
    public class ReportSetup
    {
        protected TestServer _testServer;
        protected HttpClient _client;
        private TimeregDbContext _db;
        private Action<IServiceCollection> _actions;
        private IMapper _mapper;
        private IIntegrationService _integrationService;
        private IEntityMapService _entitymapService;

        protected readonly int _customerId = 1;
        private readonly int _userId = 81730;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Common_Api_Url", "http://localhost");
            settings.Add("Integrations_Api_Url", "http://localhost");
            settings.Add("Unimicro.Api.Url", "http://test-api.unieconomy.no/api/");
            settings.Add(Unimicro.Constants.Constants.API.API_USERNAME_CONFIG_KEY, "Sticos-integration");
            settings.Add(Unimicro.Constants.Constants.API.API_PASSWORD_CONFIG_KEY, "Sticosintegration123");

            _actions = (sc) =>
            {
                _integrationService = A.Fake<IIntegrationService>();
                _entitymapService = A.Fake<IEntityMapService>();

                sc.ReplaceTransient<IDbContextFactory<TimeregDbContext>, InMemoryDbContextFactory>();
                sc.ReplaceScoped<IIntegrationService>(_integrationService);
                sc.ReplaceScoped<IEntityMapService>(_entitymapService);
            };

            _testServer = new TestServerBuilder()
                      .WithPostConfigureCollection(_actions)
                      .WithConfigSettings(settings)
                      .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);
            _mapper = _testServer.Host.Services.GetService<IMapper>();
        }

        public int AddIntegrationServiceReturnsInvalid(int unitId)
        {
            int uniqueId = new Random().Next(10, 1000000);
            A.CallTo(() =>
                _integrationService.Search(A<SearchQueryIntegration>.That.Matches(sq =>
                    sq.Category == (int)Integrations.Api.Contracts.Category.Timereg && sq.UnitId == unitId)))
                    .Returns(new List<Integrations.Api.Contracts.Integration>
                    {
                    new Integrations.Api.Contracts.Integration
                    {
                        Id = uniqueId,
                        Category = (int) Category.Timereg,
                        ExternalSystem = (int) ExternalEconomySystem.UniMicro,
                        UnitId = unitId
                    }
                });
            return uniqueId;
        }

        public void AddValidEntityMapEmployee(int unitId, int integrationId, int localId, int externalId = 1)
        {
            var entityMap = new EntityMap
            {
                IntegrationId = integrationId,
                EntityId = localId,
                Ignored = false,
                EntityName = EntityType.Employee.ToString(),
                ExternalEntity = IdentifierEntity.WorkRelation.ToString(),
                ExternalPropertyName = IdentifierProperty.Id.ToString(),
                ExternalValue = externalId.ToString(),
            };
            A.CallTo(() => _entitymapService.SearchEntityMaps(
                    A<SearchQueryEntityMap>.That.Matches(sq =>
                        sq.LocalId == entityMap.EntityId
                        && sq.EntityName == entityMap.EntityName
                        && sq.UnitId == unitId
                        && sq.ExternalEntity == entityMap.ExternalEntity
                        && sq.ExternalProperty == entityMap.ExternalPropertyName)))
                .Returns(new List<EntityMap> { entityMap });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _db.Database.EnsureDeleted();
            _testServer.Dispose();
            _client.Dispose();
        }
    }
}
