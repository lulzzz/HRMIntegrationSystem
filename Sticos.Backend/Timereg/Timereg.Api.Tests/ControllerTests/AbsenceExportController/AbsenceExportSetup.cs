using AutoMapper;
using Common.Api.Contracts;
using Common.Api.Contracts.Services;
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
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shared.Interfaces.Queries;
using Timereg.Api.Domain.Models;
using Timereg.Api.Repositories.Context;
using Timereg.Api.Repositories.ContextFactory;
using Timereg.Api.Unimicro.Models;
using entities = Timereg.Api.Repositories.Models;

namespace Timereg.Api.Tests.ControllerTests.AbsenceExportController
{
    public class AbsenceExportSetup
    {
        protected TestServer _testServer;
        protected HttpClient _client;
        private TimeregDbContext _db;
        private Action<IServiceCollection> _actions;
        private IMapper _mapper;
        private IIntegrationService _integrationService;
        private IUnitService _unitService;
        private IEntityMapService _entitymapService;
        
        protected readonly int _customerId = 1;
        private readonly int _userId = 81730;
        private IUnitQueries _unitQueries;

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
                _unitService = A.Fake<IUnitService>();
                _entitymapService = A.Fake<IEntityMapService>();
                _unitQueries = A.Fake<IUnitQueries>();

                sc.ReplaceTransient<IDbContextFactory<TimeregDbContext>, InMemoryDbContextFactory>();
                sc.ReplaceScoped<IUnitQueries>(_unitQueries);
                sc.ReplaceScoped<IIntegrationService>(_integrationService);
                sc.ReplaceScoped<IEntityMapService>(_entitymapService);
                sc.ReplaceScoped<IUnitService>(_unitService);
            };

            _testServer = new TestServerBuilder()
                      .WithPostConfigureCollection(_actions)
                      .WithConfigSettings(settings)
                      .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(_customerId, _userId);

            var dbFactory = _testServer.Host.Services.GetService<IDbContextFactory<TimeregDbContext>>();
            _mapper = _testServer.Host.Services.GetService<IMapper>();
            _db = await dbFactory.CreateDbContext();
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

        public entities.AbsenceExport AddInitialAbsence()
        {
            var employeeId = new Random().Next(100, 10000000);
            var localAbsenceId = new Random().Next(100, 10000000);

            var absence = new Absence()
            {

                LocalId = 69,
                UnitId = 1,
                EmployeeId = employeeId,
                AbsenceEntries = new List<AbsenceEntry>
            {
                new AbsenceEntry
                {
                    ExternalAbsenceCode = null,
                    ExternalEntityId = null,
                    ExternalId = null,
                    LocalAbsenceCode = localAbsenceId,
                    IsFullDay = true,
                }
            }
            };

            var uniqueId = Guid.NewGuid().ToString();
            var uniqueMessage = Guid.NewGuid().ToString();

            var absenceExport = new AbsenceExport()
            {
                Id = uniqueId.ToString(),
                UnitId = 1,
                EmployeeId = employeeId,
                Status = AbsenceExportStatus.Obsolete,
                Absence = absence,
                LocalAbsenceId = localAbsenceId,
                Message = uniqueMessage,

            };
            var mappedEntity = _mapper.Map<AbsenceExport, entities.AbsenceExport>(absenceExport);

            var absenceExportCreated = _db.AbsenceExports.Add(mappedEntity);
            _db.SaveChanges();

            return mappedEntity;
        }

        public int AddValidIntegration(int unitId)
        {
            var uniqueId = new Random().Next(1000, 10000000);

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
                        IsActivated = true,
                        UnitId = unitId
                    }
                });

            A.CallTo(() => _unitService.GetUnit(unitId))
                .Returns(Task.FromResult(new Unit()
                {
                    Id = 1,
                    Name = "Sticos AS",
                    LegalOrganizationNumber = "934228391",
                    BusinessOrganizationNumber = "934228391"
                }));
            return uniqueId;
        }

        public void AddValidEntityMapAbsenceCode(int unitId, int integrationId, int absenceCode, int externalId = 4)
        {
            var entityMap = new EntityMap
            {
                IntegrationId = integrationId,
                EntityId = absenceCode,
                Ignored = false,
                EntityName = EntityType.AbsenceType.ToString(),
                ExternalEntity = IdentifierEntity.WorkType.ToString(),
                ExternalPropertyName = IdentifierProperty.Id.ToString(),
                ExternalValue = externalId.ToString()
            };
            A.CallTo(() =>
                    _entitymapService.SearchEntityMaps(
                        A<SearchQueryEntityMap>.That.Matches(sq =>
                            sq.LocalId == entityMap.EntityId
                            && sq.EntityName == entityMap.EntityName
                            && sq.UnitId == unitId
                            && sq.ExternalEntity == entityMap.ExternalEntity
                            && sq.ExternalProperty == entityMap.ExternalPropertyName)))
                .Returns(new List<EntityMap> { entityMap });
        }

        public async Task<AbsenceExport> GetAbsenceExport(string id)
        {
            _db = await GetContext();

            var absenceExport = _db.AbsenceExports.FirstOrDefault(x => x.Id == id);
            var absenceExportMapped = _mapper.Map<AbsenceExport>(absenceExport);
            return absenceExportMapped;
        }

        public async Task<entities.AbsenceExport> AddRandomAbsenceExport(string id, int unitId, int localId)
        {
            var absenceExport = new entities.AbsenceExport
            {
                Id = id,
                UnitId = unitId,
                LocalAbsenceId = localId
            };

           await AddAbsenceExport(absenceExport);

            return absenceExport;
        }

        public async Task<TimeregDbContext> GetContext()
        {
            var context = _testServer.Host.Services.GetService<IDbContextFactory<TimeregDbContext>>();
            return await context.CreateDbContext();
        }

        public async Task AddAbsenceExport(entities.AbsenceExport absenceExport)
        {
            _db.AbsenceExports.Add(absenceExport);
            await _db.SaveChangesAsync();
        }

        public async Task AddAbsenceExportRange(List<entities.AbsenceExport> absenceExports)
        {
            _db.AbsenceExports.AddRange(absenceExports);
            await _db.SaveChangesAsync();
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
