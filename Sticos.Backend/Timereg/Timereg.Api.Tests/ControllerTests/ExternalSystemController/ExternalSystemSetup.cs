using AutoMapper;
using Common.Api.Contracts;
using commonContracts = Common.Api.Contracts.Employees;
using Common.Api.Contracts.Services;
using FakeItEasy;
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
using System.Text;
using System.Threading.Tasks;
using Timereg.Api.Repositories.Context;
using Timereg.Api.Repositories.ContextFactory;
using Timereg.Api.Unimicro.HttpClients;
using Timereg.Api.Unimicro.Models;
using commonEntities = Common.Api.Domain.Entities;

namespace Timereg.Api.Tests.ControllerTests.ExternalSystemController
{
    public class ExternalSystemSetup
    {
        protected TestServer _testServer;
        protected HttpClient _client;
        private TimeregDbContext _db;
        private Action<IServiceCollection> _actions;
        private IMapper _mapper;
        private IUnitService _unitService;
        private IUnimicroClient _unimicroClient;
        private commonContracts.IEmployeeService _employeeService;
        private IAbsenceTypeService _absenceTypeService;

        public static int CustomerId = 1;
        private readonly int _userId = 81730;

        public ExternalSystemSetup()
        {
            SetUp();
        }
        [OneTimeSetUp]
        public async Task SetUp()
        {
            var settings = new Dictionary<string, string>();
            settings.Add("Unimicro.Api.Url", "http://dummyUrl");
            settings.Add("Common_Api_Url", "http://dummyUrl");
            settings.Add("Integrations_Api_Url", "http://dummyUrl");

            _actions = (sc) =>
            {
                _unitService = A.Fake<IUnitService>();
                _employeeService = A.Fake<commonContracts.IEmployeeService>();
                _absenceTypeService = A.Fake<IAbsenceTypeService>();
                _unimicroClient = A.Fake<IUnimicroClient>();
                A.CallTo(() => _unimicroClient.SignIn()).Returns(new Login {AccessToken = "accessToken"});

                sc.ReplaceTransient<IDbContextFactory<TimeregDbContext>, InMemoryDbContextFactory>();
                sc.ReplaceScoped<IUnitService>(_unitService);
                sc.ReplaceScoped<commonContracts.IEmployeeService>(_employeeService);
                sc.ReplaceScoped<IAbsenceTypeService>(_absenceTypeService);
                sc.ReplaceScoped<IUnimicroClient>(_unimicroClient);
            };

            _testServer = new TestServerBuilder()
                         .WithPostConfigureCollection(_actions)
                         .WithConfigSettings(settings)
                         .Build<Startup>();

            _client = _testServer.CreateClientWithJwtToken(CustomerId, _userId);
            _mapper = _testServer.Host.Services.GetService<IMapper>();
            var dbFactory = _testServer.Host.Services.GetService<IDbContextFactory<TimeregDbContext>>();
            _db = await dbFactory.CreateDbContext();
        }

        public ExternalSystemSetup WithExternalEmployees(string organizationNumber, List<Employee> employees)
        {
            A.CallTo(() => _unimicroClient.GetEmployees(organizationNumber)).Returns(employees);
            return this;
        }

        public ExternalSystemSetup WithExternalEmployments(List<Employment> employments)
        {
            A.CallTo(() => _unimicroClient.GetEmployments(A<List<int>>.Ignored)).Returns(employments);
            return this;
        }
        public ExternalSystemSetup WithExternalWorkers(List<Worker> workers)
        {
            A.CallTo(() => _unimicroClient.GetWorkers(A<List<int>>.Ignored)).Returns(workers);
            return this;
        }
            
        public ExternalSystemSetup WithExternalWorkerRelations(List<WorkRelation> workRelations)
        {
            A.CallTo(() => _unimicroClient.GetWorkRelations(A<List<int>>.Ignored)).Returns(workRelations);
            return this;
        }
        public ExternalSystemSetup WithSticosUnit(Unit unit)
        {
            A.CallTo(() => _unitService.GetUnit(unit.Id)).Returns(unit);
            return this;
        }

        public void AddEmployeeServiceData(string name, int id)
        {
            A.CallTo(() => _employeeService.SearchEmployee(A<commonContracts.SearchQueryEmployee>.That.Matches(sq => sq.EmployeesIds.Contains(id))))
           .Returns(new List<commonContracts.IEmployee>
           {
               new commonContracts.Employee
               {
                   Id = id,
                   FirstName = name
               }
           });
        }

        public void AddMilitaryLeaveType(int id)
        {
            A.CallTo(() => _absenceTypeService.GetAbsenceTypes(A<SearchQueryAbsenceType>.That.Matches(sq => sq.AbsenceTypesIds.Contains(id))))
   .Returns(new List<commonEntities.AbsenceType>
   {
               new commonEntities.AbsenceType
               {
                  SpecificValue = Sticos.Personal.MessageContracts.Enums.AbsenceSubType.MilitaryLeave,
               }
   });
        }

        public static Unit ValidSticosUnit = new Unit()
        {
            Id = 1,
            Name = "Sticos AS",
            LegalOrganizationNumber = "934228391",
            BusinessOrganizationNumber = "971998016"
        };

        public static Employee ValidEmployee1 => new Employee {Id = 1};
        public static Employment ValidEmployment1 => new Employment { Id = 1, EmployeeId = ValidEmployee1.Id, WorkPercent = 100,HoursPerWeek = 37.5,StartDate = DateTime.Now.AddMonths(-1),EndDate = DateTime.Now.AddYears(100)};
        public static Worker ValidWorker1 => new Worker() { Id = 1, EmployeeId = ValidEmployee1.Id};
        public static WorkRelation ValidWorkRelation1 => new WorkRelation() { Id = 1, WorkerID = ValidWorker1.Id, IsActive = true,StartDate = DateTime.Now.AddMonths(-1),EndDate = DateTime.Now.AddYears(100),WorkPercentage = 100, WorkProfile = new WorkProfile{MinutesPerWeek = 2250}};

        public HttpClient BuildClient()
        {
            return _client;
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
