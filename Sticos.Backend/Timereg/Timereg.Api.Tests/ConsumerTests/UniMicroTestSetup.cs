using AutoMapper;
using Common.Api.Contracts;
using Common.Api.Contracts.Employees;
using Common.Api.Contracts.Services;
using FakeItEasy;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Sticos.Personal.MessageContracts;
using Sticos.Personal.MessageContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared.Interfaces.Queries;
using Shared.Services;
using Shared.Services.Extensions;
using Shared.Services.Models;
using Timereg.Api.Domain.Exceptions;
using Timereg.Api.Domain.Models;
using Timereg.Api.Extensions;
using Timereg.Api.MessageBus;
using Timereg.Api.Repositories.Context;
using Timereg.Api.Repositories.ContextFactory;
using Timereg.Api.Unimicro.Constants;
using Timereg.Api.Unimicro.HttpClients;
using Timereg.Api.Unimicro.Models;
using AbsenceType = Sticos.Personal.MessageContracts.Enums.AbsenceType;
using common = Common.Api.ProxyClient.Client;
using Timereg.Api.Unimicro.Adapters.UniMicro;
using Shared.MessageBus.Contracts;
using Shared.Services.Services;
using Integration = Integrations.Api.Contracts.Integration;

namespace Timereg.Api.Tests.ConsumerTests
{
    public class UniMicroTestSetup
    {
        private IIntegrationService _integrationService;
        private IUnimicroClient _uniMicroClient;
        private IUnitService _unitService;
        private readonly ServiceProvider _services;
        private readonly IDbContextFactory<TimeregDbContext> _dbFactory;
        private readonly TimeregDbContext _db;
        private readonly IMapper _mapper;
        //private int _unitId;
        private int _externalWorkItemIds;
        private readonly int _externalWorkRelationId;
        private IEntityMapService _entityMapService;
        private static int _integrationId = 1337;
        private int _externalEmploymentLeaveIds;
        private IUnitQueries _unitQueries;

        public UniMicroTestSetup()
        {
            _services = BuildServices();
            _dbFactory = _services.GetService<IDbContextFactory<TimeregDbContext>>();

            // Poor mans database-reset
            _dbFactory.CreateDbContext().Result.Database.EnsureDeleted();

            _db = _dbFactory.CreateDbContext().Result;
            _mapper = _services.GetService<IMapper>();
            _externalWorkItemIds = 1;
            _externalEmploymentLeaveIds = 101;
        }

        public ServiceProvider BuildServices()
        {
            var services = new ServiceCollection();
            var baseUrl = "http://localhost:5000/";
            var configSettings = new Dictionary<string, string>()
            {
                {common.Constants.API_URL_CONFIG_KEY,baseUrl},
                {Constants.API.API_URL_CONFIG_KEY, baseUrl},
                {Integrations.Api.ProxyClient.Client.Constants.API_URL_CONFIG_KEY, baseUrl},
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configSettings)
                .Build();

            // Set up default services

            services.AddScoped<IConfiguration>(c => configuration);
            services.AddIocMapping();
            services.AddAutoMapper(AutoMapperSetup.Config);
            services.AddHttpClients(configuration);
            services.AddSharedServices();

            var userContext = new StaticUserContext(new UserContext { UserId = 81730 });
            services.AddSingleton<ICurrentUserContext>(userContext);

            _integrationService = A.Fake<IIntegrationService>();
            _uniMicroClient = A.Fake<IUnimicroClient>();
            _unitService = A.Fake<IUnitService>();
            _entityMapService = A.Fake<IEntityMapService>();
            _unitQueries = A.Fake<IUnitQueries>();
            var absenceTypeService = A.Fake<IAbsenceTypeService>();
            var fakeEmployeeService = A.Fake<IEmployeeService>();
            var fakeAuthorizationContext = A.Fake<IAuthorizationContextService>();

            services.ReplaceScoped<IUnitQueries>(_unitQueries);
            services.ReplaceScoped<IUnimicroClient>(_uniMicroClient);
            services.ReplaceScoped<IEntityMapService>(_entityMapService);
            services.ReplaceScoped<IUnitService>(_unitService);
            services.ReplaceScoped<IEmployeeService>(fakeEmployeeService);
            services.ReplaceScoped<IAbsenceTypeService>(absenceTypeService);
            services.ReplaceScoped<IAuthorizationContextService>(fakeAuthorizationContext);
            services.ReplaceSingleton(_integrationService);

            services.ReplaceScoped<IDbContextFactory<TimeregDbContext>, InMemoryDbContextFactory>();

            services.AddScoped<IConsumer<IAbsenceApproved>, AbsenceApprovedConsumer>();
            services.AddScoped<IConsumer<IAbsenceDeleted>, AbsenceDeletedConsumer>();
            services.AddScoped<IConsumer<IIntegrationDeleted>, IntegrationDeleteConsumer>();
            services.AddScoped<IConsumer<IEmployeeDeleted>, EmployeeDeletedConsumer>();

            return services.BuildServiceProvider();
        }

        private static ConsumeContext<T> CreateConsumeContextWithMessage<T>(T message) where T : class
        {
            var fakeContext = A.Fake<ConsumeContext<T>>();
            A.CallTo(() => fakeContext.Message).Returns(message);
            return fakeContext;
        }

        public static ConsumeContext<IAbsenceApproved> ValidMessageParentalLeave => CreateConsumeContextWithMessage(new AbsenceMessageData
        {
            AbsenceId = 69,
            CreatedDate = DateTime.Now,
            UnitId = 1000,
            EmployeeId = 87889,
            AbsenceStatus = AbsenceStatus.Approved,
            AbsenceEntries = new List<IAbsenceEntry>
            {
                new AbsenceEntry
                {
                    FromDate =  new DateTime(2018, 8, 1).Date,
                    ToDate = new DateTime(2018, 8, 12).Date,
                    AbsenceType = AbsenceType.Leave,
                    AbsenceSubType = AbsenceSubType.ParentalLeave,
                    IsFullDay = true
                }
            }
        });
        
        public static Integrations.Api.Contracts.Integration GetValidIntegration(int unitId)
        {
            return new Integrations.Api.Contracts.Integration
            {
                Id = _integrationId,
                Category = (int) Category.Timereg,
                ExternalSystem = (int) ExternalEconomySystem.UniMicro,
                IsActivated = true,
                UnitId = unitId
            };
        }

        public static Unit GetValidUnit(int unitId, int? parentId=null)
        {
            return new Unit()
            {
                Id = unitId,
                Name = "Test",
                BusinessOrganizationNumber = "999999999",
                ParentId = parentId
            };
        }
        public UniMicroTestSetup WithUniMicroEnabledForUnitId(int unitId)
        {
            A.CallTo(() =>
                    _integrationService.Search(A<SearchQueryIntegration>.That.Matches(sq =>
                        sq.Category == (int)Integrations.Api.Contracts.Category.Timereg && sq.UnitId == unitId)))
                .Returns(new List<Integrations.Api.Contracts.Integration>
                {
                   GetValidIntegration(unitId)
                });

            A.CallTo(() => _unitService.GetUnit(unitId))
                .Returns(GetValidUnit(unitId));

            return this;
        }

        public static ConsumeContext<IAbsenceApproved> ValidApprovedMessageToday => CreateConsumeContextWithMessage<IAbsenceApproved>(_messageDataToday);
        public static ConsumeContext<IAbsenceDeleted> ValidDeletedMessageToday => CreateConsumeContextWithMessage<IAbsenceDeleted>(_messageDataToday);

        private static AbsenceMessageData _messageDataToday = new AbsenceMessageData
        {
            AbsenceId = 69,
            CreatedDate = DateTime.Now,
            UnitId = 1000,
            EmployeeId = 87889,
            AbsenceStatus = AbsenceStatus.Approved,
            AbsenceEntries = new List<IAbsenceEntry>
            {
                new AbsenceEntry
                {
                    FromDate =  DateTime.Now.Date.AddHours(8),
                    ToDate = DateTime.Now.Date.AddHours(15).AddMinutes(30),
                    AbsenceType = AbsenceType.Leave,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                }
            }
        };
        public static ConsumeContext<IAbsenceApproved> ValidMessageWeekVacation => CreateConsumeContextWithMessage(new AbsenceMessageData
        {
            AbsenceId = new Random().Next(0, 100),
            CreatedDate = DateTime.Now,
            UnitId = 1000,
            EmployeeId = 87889,
            AbsenceStatus = AbsenceStatus.Approved,
            AbsenceEntries = new List<IAbsenceEntry>
            {
                new AbsenceEntry
                {
                    FromDate =  DateTime.Now.Date,
                    ToDate = DateTime.Now.Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  DateTime.Now.AddDays(1).Date,
                    ToDate = DateTime.Now.AddDays(1).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  DateTime.Now.AddDays(2).Date,
                    ToDate = DateTime.Now.AddDays(2).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  DateTime.Now.AddDays(3).Date,
                    ToDate = DateTime.Now.AddDays(3).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  DateTime.Now.AddDays(4).Date,
                    ToDate = DateTime.Now.AddDays(4).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  DateTime.Now.AddDays(7).Date,
                    ToDate = DateTime.Now.AddDays(7).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  DateTime.Now.AddDays(8).Date,
                    ToDate = DateTime.Now.AddDays(8).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                }
            }
        });

        /// <summary>
        /// Er borte hele august 2018. Har permisjon hele August, med en uke ferie inni midten der.
        /// </summary>
        public static ConsumeContext<IAbsenceApproved> ValidMessageOneMonthParentalLeaveWithVacation => CreateConsumeContextWithMessage(new AbsenceMessageData
        {
            AbsenceId = new Random().Next(0, 100),
            CreatedDate = DateTime.Now,
            UnitId = 1000,
            EmployeeId = 87889,
            AbsenceStatus = AbsenceStatus.Approved,
            AbsenceEntries = new List<IAbsenceEntry>
            {
                new AbsenceEntry
                {
                    FromDate =  new DateTime(2018, 8, 1).Date,
                    ToDate = new DateTime(2018, 8, 12).Date,
                    AbsenceType = AbsenceType.Leave,
                    AbsenceSubType = AbsenceSubType.ParentalLeave,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  new DateTime(2018, 8, 13).Date,
                    ToDate = new DateTime(2018, 8, 13).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  new DateTime(2018, 8, 14).Date,
                    ToDate = new DateTime(2018, 8, 14).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  new DateTime(2018, 8, 15).Date,
                    ToDate = new DateTime(2018, 8, 15).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },

                new AbsenceEntry
                {
                    FromDate =  new DateTime(2018, 8, 16).Date,
                    ToDate = new DateTime(2018, 8, 16).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },

                new AbsenceEntry
                {
                    FromDate =  new DateTime(2018, 8, 17).Date,
                    ToDate = new DateTime(2018, 8, 17).Date,
                    AbsenceType = AbsenceType.VacationTimeoff,
                    AbsenceSubType = AbsenceSubType.Vacation,
                    IsFullDay = true
                },
                new AbsenceEntry
                {
                    FromDate =  new DateTime(2018, 8, 15).Date,
                    ToDate = new DateTime(2018, 8, 30).Date,
                    AbsenceType = AbsenceType.Leave,
                    AbsenceSubType = AbsenceSubType.ParentalLeave,
                    IsFullDay = true
                },
            }
        });
        public UniMicroTestSetup WithUnits(List<Unit> units)
        {
            foreach (var unit in units)
            {
                A.CallTo(() => _unitService.GetUnit(unit.Id))
                    .Returns(Task.FromResult(unit));
            }
            return this;
        }

        public UniMicroTestSetup WithIntegrations(List<Integrations.Api.Contracts.Integration> integrations)
        {
            foreach (var integration in integrations)
            {
                A.CallTo(() =>
                        _integrationService.Search(A<SearchQueryIntegration>.That.Matches(sq =>
                            sq.Category == (int) Integrations.Api.Contracts.Category.Timereg &&
                            sq.UnitId == integration.UnitId)))
                    .Returns(new List<Integrations.Api.Contracts.Integration> {integration});
            }
            return this;
        }

        private List<EntityMap> _workRelationMaps = new List<EntityMap>();
        private List<EntityMap> _employmentMaps = new List<EntityMap>();
        public UniMicroTestSetup WithLocalEmployeeMappedToWorkRelation(int localId, string externalId, int unitId)
        {
            var entityMap = new EntityMap
            {
                IntegrationId = _integrationId,
                EntityId = localId,
                Ignored = false,
                EntityName = EntityType.Employee.ToString(),
                ExternalEntity = IdentifierEntity.WorkRelation.ToString(),
                ExternalPropertyName = IdentifierProperty.Id.ToString(),
                ExternalValue = externalId,
            };
            _workRelationMaps.Add(entityMap);
            A.CallTo(() => _entityMapService.SearchEntityMaps(
                    A<SearchQueryEntityMap>.That.Matches(sq =>
                        sq.LocalId == entityMap.EntityId
                        && sq.EntityName == entityMap.EntityName
                        && sq.UnitId == unitId
                        && sq.ExternalEntity == entityMap.ExternalEntity
                        && sq.ExternalProperty == entityMap.ExternalPropertyName)))
                .Returns(_workRelationMaps);

            return this;
        }

        public UniMicroTestSetup WithLocalEmployeeMappedToEmployment(int localId, string externalId, int unitId)
        {
            var entityMap = new EntityMap
            {
                IntegrationId = _integrationId,
                EntityId = localId,
                Ignored = false,
                EntityName = EntityType.Employee.ToString(),
                ExternalEntity = IdentifierEntity.Employment.ToString(),
                ExternalPropertyName = IdentifierProperty.Id.ToString(),
                ExternalValue = externalId
            };
            _employmentMaps.Add(entityMap);
            A.CallTo(() => _entityMapService.SearchEntityMaps(
                    A<SearchQueryEntityMap>.That.Matches(sq =>
                        sq.LocalId == entityMap.EntityId
                        && sq.EntityName == entityMap.EntityName
                        && sq.UnitId == unitId
                        && sq.ExternalEntity == entityMap.ExternalEntity
                        && sq.ExternalProperty == entityMap.ExternalPropertyName)))
                .Returns(_employmentMaps);

            return this;
        }

        public UniMicroTestSetup WithExistingAbsenceExport(int unitId, int localAbsenceId, int localAbsenceCode, List<int> externalAbsenceIds, string dbId = null)
        {
            var absence = new Absence
            {
                UnitId = unitId,
                LocalId = localAbsenceId,
                AbsenceEntries = externalAbsenceIds
                    .Select(id => new Domain.Models.AbsenceEntry
                    {
                        ExternalId = id.ToString(),
                        LocalAbsenceCode = localAbsenceCode,
                    })
                    .ToList()
            };

            var absenceExport = new Repositories.Models.AbsenceExport
            {
                Id = dbId,
                Status = (int)AbsenceExportStatus.Success,
                LocalAbsenceId = absence.LocalId,
                AbsenceJson = JsonConvert.SerializeObject(absence),
                Message = "Success",
                UnitId = absence.UnitId,
                Action = (int)AbsenceExportAction.Create
            };

            _db.AbsenceExports.Add(absenceExport);
            _db.SaveChanges();

            return this;
        }

        public UniMicroTestSetup WithUnitMapped(int localId, int externalId, int unitId)
        {
            var entityMap = new List<EntityMap>{new EntityMap
            {
                IntegrationId = _integrationId,
                EntityId = localId,
                Ignored = false,
                EntityName = EntityType.Unit.ToString(),
                ExternalValue = externalId.ToString(),
            }};
            A.CallTo(() => _entityMapService.SearchEntityMaps(
                A<SearchQueryEntityMap>.That.Matches(sq =>
                    sq.LocalId == localId && sq.EntityName == EntityType.Unit.ToString()
                    && sq.IntegrationId == _integrationId)))
                .Returns(entityMap);
            A.CallTo(() => _entityMapService.SearchEntityMaps(
                A<SearchQueryEntityMap>.That.Matches(sq =>
                    sq.LocalId == localId
                    && sq.EntityName == EntityType.Unit.ToString()
                    && sq.UnitId == unitId
                    && sq.ExternalSystemId == (int)ExternalEconomySystem.UniMicro
                    && sq.IntegrationCategory == (int)Category.Timereg))) //int unitId, int externalSystemId, int integrationCategory
                .Returns(entityMap);

            return this;
        }

        public UniMicroTestSetup WithAbsenceMappedToWorkType(int localId, int externalId, int unitId)
        {
            var entityMap = new EntityMap
            {
                IntegrationId = _integrationId,
                EntityId = localId,
                Ignored = false,
                EntityName = EntityType.AbsenceType.ToString(),
                ExternalEntity = IdentifierEntity.WorkType.ToString(),
                ExternalPropertyName = IdentifierProperty.Id.ToString(),
                ExternalValue = externalId.ToString()
            };
            A.CallTo(() =>
                    _entityMapService.SearchEntityMaps(
                        A<SearchQueryEntityMap>.That.Matches(sq =>
                            sq.LocalId == entityMap.EntityId
                            && sq.EntityName == entityMap.EntityName
                            && sq.UnitId == unitId
                            && sq.ExternalEntity == entityMap.ExternalEntity
                            && sq.ExternalProperty == entityMap.ExternalPropertyName)))
                .Returns(new List<EntityMap> { entityMap });

            return this;
        }



        public UniMicroTestSetup WithAbsenceMappedToLeaveType(int localId, LeaveType externalId, int unitId)
        {
            var entityMap = new EntityMap
            {
                IntegrationId = _integrationId,
                EntityId = localId,
                Ignored = false,
                EntityName = EntityType.AbsenceType.ToString(),
                ExternalEntity = IdentifierEntity.EmploymentLeaveType.ToString(),
                ExternalPropertyName = IdentifierProperty.Id.ToString(),
                ExternalValue = ((int)externalId).ToString()
            };
            A.CallTo(() =>
                    _entityMapService.SearchEntityMaps(
                        A<SearchQueryEntityMap>.That.Matches(sq =>
                            sq.LocalId == entityMap.EntityId
                            && sq.EntityName == entityMap.EntityName
                            && sq.UnitId == unitId
                            && sq.ExternalEntity == entityMap.ExternalEntity
                            && sq.ExternalProperty == entityMap.ExternalPropertyName)))
                .Returns(new List<EntityMap> { entityMap });

            return this;
        }

        public (IConsumer<IAbsenceApproved>, IDbContextFactory<TimeregDbContext>) SetupApprovedConsumer()
        {
            return (_services.GetService<IConsumer<IAbsenceApproved>>(), _dbFactory);
        }

        public (IConsumer<IAbsenceDeleted>, IDbContextFactory<TimeregDbContext>) SetupDeletedConsumer()
        {
            return (_services.GetService<IConsumer<IAbsenceDeleted>>(), _dbFactory);
        }

        public (IConsumer<IAbsenceApproved>, IDbContextFactory<TimeregDbContext>, IUnimicroClient) SetupExposingClient()
        {
            return (_services.GetService<IConsumer<IAbsenceApproved>>(), _dbFactory, _uniMicroClient);
        }

        public (IConsumer<IIntegrationDeleted>, IDbContextFactory<TimeregDbContext>) SetupIntegrationDeleteConsumer()
        {
            return (_services.GetService<IConsumer<IIntegrationDeleted>>(), _dbFactory);
        }

        public UniMicroTestSetup DeleteCallFails()
        {
            A.CallTo(() => _uniMicroClient.DeleteWorkItem(A<string>.Ignored))
                    .Throws(new ExternalSystemCommunicationException("Error requesting Unimicro-Api"));
            return this;
        }

        public UniMicroTestSetup UniMicroServiceReturnsWorkItem(IAbsenceMessageData message)
        {
            var absence = MessageToAbsenceMapper.CreateAbsenceFromMessage(message);

            foreach (var absenceEntry in absence.AbsenceEntries)
            {
                if (absenceEntry.LocalAbsenceCode == (int)AbsenceSubType.ParentalLeave)
                {
                    var employementId = _entityMapService.GetEmploymentId(absence.EmployeeId, absence.UnitId).Result.Value;
                    var leaveTypeId = _entityMapService.GetEmploymentLeaveId(absenceEntry.LocalAbsenceCode, absence.UnitId).Result.Value;

                    A.CallTo(() => _uniMicroClient.PostEmploymentLeave(A<EmploymentLeave>
                            .That.Matches(w =>
                                w.FromDate == absenceEntry.StartTime
                                && w.ToDate == absenceEntry.EndTime
                                && w.EmploymentID == employementId
                                && w.LeaveType == (LeaveType)leaveTypeId
                            )))
                        .ReturnsLazily(() => Task.FromResult(_externalEmploymentLeaveIds++));
                }
                else
                {
                    A.CallTo(() => _uniMicroClient.PostWorkItem(A<WorkItem>.That.Matches(w =>
                        w.StartTime == absenceEntry.StartTime
                        && w.EndTime == absenceEntry.EndTime
                        && w.WorkRelationId == _entityMapService.GetWorkRelationId(absence.EmployeeId, absence.UnitId).Result.Value
                        && w.WorkTypeId == _entityMapService.GetWorkTypeId(absenceEntry.LocalAbsenceCode, absence.UnitId).Result.Value
                    ))).ReturnsLazily(() => Task.FromResult(_externalWorkItemIds++));
                }
            }

            return this;
        }

        private static IntegrationDeleted _timeregIntegrationDeletedMessage = new IntegrationDeleted
        {
            Id = 1,
            UnitId = 1000,
            ExternalSystem = 1,
            Category = 1
        };

        public static ConsumeContext<IIntegrationDeleted> TimeregIntegartionDeletedMessage => CreateConsumeContextWithMessage(_timeregIntegrationDeletedMessage);

        private static IntegrationDeleted _otherIntegrationDeletedMessage = new IntegrationDeleted
        {
            Id = 1,
            UnitId = 1000,
            ExternalSystem = 1,
            Category = 2
        };

        public static ConsumeContext<IIntegrationDeleted> OtherIntegartionDeletedMessage => CreateConsumeContextWithMessage(_otherIntegrationDeletedMessage);

        public UniMicroTestSetup UniMicroServiceFails()
        {
            A.CallTo(() => _uniMicroClient.PostWorkItem(A<WorkItem>.Ignored))
                    .Throws(new ExternalSystemCommunicationException("Error requesting Unimicro-Api"));
            return this;
        }

        private static EmployeeDeleted _timeregEmployeeDeletedMessage = new EmployeeDeleted
        {
            EmployeeId = 1,
            OrgUnitId = 12345,
            UserId = 10
        };

        public static ConsumeContext<IEmployeeDeleted> TimeregEmployeeDeletedMessage => CreateConsumeContextWithMessage(_timeregEmployeeDeletedMessage);

        public (IConsumer<IEmployeeDeleted>, IDbContextFactory<TimeregDbContext>) SetupEmployeeDeleteConsumer()
        {
            return (_services.GetService<IConsumer<IEmployeeDeleted>>(), _dbFactory);
        }


        public class AbsenceMessageData : IAbsenceApproved, IAbsenceDeleted
        {
            public int AbsenceId { get; set; }
            public int UnitId { get; set; }
            public int EmployeeId { get; set; }
            public int CustomerId { get; set; }
            public AbsenceStatus AbsenceStatus { get; set; }
            public DateTime CreatedDate { get; set; }
            public List<IAbsenceEntry> AbsenceEntries { get; set; }
        }

        public class AbsenceEntry : IAbsenceEntry
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public bool IsFullDay { get; set; }
            public AbsenceType AbsenceType { get; set; }
            public AbsenceSubType AbsenceSubType { get; set; }
        }

        public class IntegrationDeleted : IIntegrationDeleted
        {
            public int Id { get; set; }
            public int UnitId { get; set; }
            public int ExternalSystem { get; set; }
            public int Category { get; set; }
            public int CustomerId { get; set; }
        }
    }
}
