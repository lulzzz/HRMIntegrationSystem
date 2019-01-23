using FakeItEasy;

using Altinn.Api.Domain.Interfaces;
using Altinn.Api.Domain.Entities;
using Altinn.Api.Client.Adapters;

using Common.Api.Contracts.Services;
using Common.Api.Contracts.Employees;
using Shared.Interfaces;

namespace Altinn.Api.TestCommon.Builders
{
    public class NavMessageServiceBuilder
    {
        private IRepository<NavMessage, SearchQueryNavMessage> _navMessageRepository = A.Fake<IRepository<NavMessage, SearchQueryNavMessage>>();
        private IAltinnAdapter _altinnAdapter = A.Fake<IAltinnAdapter>();
        private ICurrentUserContext _userService = A.Fake<ICurrentUserContext>();
        private IUnitService _companyService = A.Fake<IUnitService>();
        private IAbsenceService _absenceService = A.Fake<IAbsenceService>();
        private IEmployeeService _employeeService = A.Fake<IEmployeeService>();
        private IXmlSerializer _xmlSerializer = A.Fake<IXmlSerializer>();

        public NavMessageServiceBuilder WithNavMessageRepository(IRepository<NavMessage, SearchQueryNavMessage> navMessageRepository)
        {
            _navMessageRepository = navMessageRepository;
            return this;
        }
        public NavMessageServiceBuilder WithNavMessageIntegrator(IAltinnAdapter altinnAdapter)
        {
            _altinnAdapter = altinnAdapter;
            return this;
        }
        public NavMessageServiceBuilder WithCurrentUserService(ICurrentUserContext userService)
        {
            _userService = userService;
            return this;
        }
        public NavMessageServiceBuilder WithOrgUnitService(IUnitService companyService)
        {
            _companyService = companyService;
            return this;
        }
        public NavMessageServiceBuilder WithEmployeeService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            return this;
        }
        public NavMessageServiceBuilder WithFraværService(IAbsenceService absenceService)
        {
            _absenceService = absenceService;
            return this;
        }
        public NavMessageServiceBuilder WithXmlSerializer(IXmlSerializer xmlSerializer)
        {
            _xmlSerializer = xmlSerializer;
            return this;
        }
        //public NavMessageService Build()
        //{
        //    return new NavMessageService(_navMessageRepository, /*_altinnAdapter*/ _userService, _companyService, _employeeService,  /*_absenceService,*/ _xmlSerializer);
        //}
    }
}