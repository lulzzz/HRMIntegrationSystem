using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Common.Api.Contracts.Services;
using Common.Api.Contracts.Employees;
using Shared.Interfaces;

using Altinn.Api.Domain.Entities;
using Altinn.Api.Domain.Interfaces;
using Altinn.Api.Domain.Schemas;
using Altinn.Api.Client.Adapters;

namespace Altinn.Api.Services
{
    public class NavMessageService : INavMessageService
    {
        private readonly IRepository<NavMessage, SearchQueryNavMessage> _repository;
        private readonly IAltinnAdapter _adapter;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IUnitService _companyService;
        private readonly IEmployeeService _employeeService;
        //private readonly IAbsenceService _absenceService;
        private readonly IXmlSerializer _serializer;

        public const string Sykmelding_Namespace = "http://nav.no/melding/virksomhet/sykmeldingArbeidsgiver/v1/sykmeldingArbeidsgiver";
        public const string OppgiLeder_Namespace = "http://seres.no/guid/NAV/Meldingsmodell/OppgiPersonalleder_M/630183";

        private List<string> _approvedMessageTypes = new List<string> { Sykmelding_Namespace, OppgiLeder_Namespace };

        public NavMessageService(IRepository<NavMessage, SearchQueryNavMessage> repository,
            IAltinnAdapter integrator,
            ICurrentUserContext currentUserContext,
            IUnitService companyService,
            IEmployeeService employeeService,
            //IAbsenceService absenceService,
            IXmlSerializer xmlSerializer)
        {
            _repository = repository;
            _adapter = integrator;
            _currentUserContext = currentUserContext;
            _companyService = companyService;
            _employeeService = employeeService;
            //_absenceService = absenceService;
            _serializer = xmlSerializer;
        }

        public async void ImportMessages(string businessOrganizationNumber)
        {
            // Import new messages from external party (NAV)

            var importMessages = await _adapter.ReadMessages(businessOrganizationNumber);

            foreach (var navMessages in importMessages)
            {
                await _repository.Save(navMessages);
            }
        }

        public async void ExportMessages(string businessOrganizationNumber)
        {
            //Fetch all messages ready for export and send them to external party (NAV)

            var searchMessages = await Search(new SearchQueryNavMessage
            {
                BusinessOrganizationNumber = businessOrganizationNumber,
                IntegrationType = IntegrationType.Export,
                WorkState = WorkState.ReadyforExport
            });

            var exportMessages = await _adapter.WriteMessages(businessOrganizationNumber, searchMessages);

            foreach (var navMessage in exportMessages)
            {
                navMessage.WorkState = WorkState.CompletedSuccessfully;
                await _repository.Save(navMessage);
            }
        }

        public async Task<IEnumerable<NavMessage>> Search(SearchQueryNavMessage searchQuery)
        {
            return await _repository.Search(searchQuery);
        }

        private async Task ProcessMessages()
        {
            var businessOrganizationNumber = await GetBusinessOrganizationNumberForCurrentUser();
            ProcessMessages(businessOrganizationNumber);
        }

        public async void ProcessMessages(string businessOrganizationNumber)
        {
            //todo: add guards/nullchecks. flow below is mostly "happy-path". various checks must be added
            if (string.IsNullOrWhiteSpace(businessOrganizationNumber)) return;

            // Fetch imported messages ready for processsing in database
            var messages = GetActiveImportedNavMessages(businessOrganizationNumber);

            foreach (var navMessage in await messages)
            {
                ValidateSupportedMessage(navMessage.Namespace);

                var sm = _serializer.Deserialize<SykmeldingArbeidsgiver>(navMessage.MessageXml);

                var employee = GetEmployee(sm);

                var hrMananger = GetHrManager(employee);
                var hrMessage = CreateHrManagerMessage(navMessage.Id.ToString(), businessOrganizationNumber, hrMananger, sm);
                await _repository.Save(hrMessage);

                //_absenceService.CreateAbsence(sm, employee);

                navMessage.WorkState = WorkState.CompletedSuccessfully;
                await _repository.Save(navMessage);
            }
        }

        private async Task<string> GetBusinessOrganizationNumberForCurrentUser()
        {
            var userId = _currentUserContext.Get().UserId;

            var employees = await _employeeService.SearchEmployee(new SearchQueryEmployee { UserIds = new List<int> { userId } });
            var employee = employees.FirstOrDefault(e => e.UnitId.HasValue);
            if (employee != null)
            {
                var orgNumber = await _companyService.GetUnit(employee.UnitId.Value);
                return orgNumber.BusinessOrganizationNumber;
            }

            return string.Empty;
        }

        private object GetHrManager(IEmployee employee)
        {
            //todo: Find "HR manager" for the given employee
            return null;
        }

        private IEmployee GetEmployee(SykmeldingArbeidsgiver sm)
        {
            var sykmeldingPasient = sm.sykmelding.pasient;
            var socialSecurityNumber = sykmeldingPasient.ident;
            IEmployee employee = null;
            var employeeSearchParameters = new SearchQueryEmployee() { SocialSecurityNumber = socialSecurityNumber };

            employee = _employeeService.SearchEmployee(employeeSearchParameters).Result.FirstOrDefault();
            if (employee == null)
            {
                var possiblEmployeeMatch = _employeeService.SearchEmployee(new SearchQueryEmployee
                {
                    FirstName = sykmeldingPasient.navn.fornavn,
                    LastName = sykmeldingPasient.navn.etternavn
                });
                employee = possiblEmployeeMatch.Result.FirstOrDefault();
            }
            return employee;
        }

        private NavMessage CreateHrManagerMessage(string referenceId, string businessOrganizationNumber, object hrMananger, SykmeldingArbeidsgiver sm)
        {
            var sykmeldingPasient = sm.sykmelding.pasient;
            var socialSecurityNumber = sykmeldingPasient.ident;

            //todo: check if this business is paying salary to the employee in the employer-period (arbeidsgiverperioden, first 16days)
            bool businessPayingSalaryInEmployerPeriod = true;
            //todo: Create a new "OppgiPersonalleder"/(Provide HR Manager)-message  and persist in database
            var provideHrManagerMessage = new OppgiPersonalleder_M()
            {
                Skjemainnhold = new Skjemainnhold()
                {
                    hendelseId = sm.sykmeldingId,
                    naermesteLeder = new NaermesteLeder() { }, //map data from hrManager
                    organisasjonsnummer = sm.virksomhetsnummer,
                    utbetalesLonn = businessPayingSalaryInEmployerPeriod,
                    utbetalesLonnSpecified = true,
                    sykmeldt = new Sykmeldt()
                    {
                        sykmeldtFoedselsnummer = socialSecurityNumber,
                        sykmeldtNavn = $"{sykmeldingPasient.navn.fornavn} {sykmeldingPasient.navn.mellomnavn} {sykmeldingPasient.navn.etternavn}",
                    }
                }
            };
            var messageXml = _serializer.Serialize(provideHrManagerMessage);

            var navHrManagerMessage = new NavMessage()
            {
                MessageXml = messageXml,
                Namespace = OppgiLeder_Namespace,
                IntegrationType = IntegrationType.Export,
                WorkState = WorkState.ReadyforExport,
                ReferenceId = referenceId,
                BusinessOrganizationNumber = businessOrganizationNumber
            };
            return navHrManagerMessage;
        }

        private async Task<IEnumerable<NavMessage>> GetActiveImportedNavMessages(string businessOrganizationNumber)
        {
            var navMessageSearchParameters = new SearchQueryNavMessage
            {
                Namespace = Sykmelding_Namespace,
                IntegrationType = IntegrationType.Import,
                WorkState = WorkState.ReadyForProcessing,
                BusinessOrganizationNumber = businessOrganizationNumber,
            };
            var messages = await _repository.Search(navMessageSearchParameters);
            return messages;
        }

        private void ValidateSupportedMessage(string messageNamespace)
        {
            if (string.IsNullOrWhiteSpace(messageNamespace) || !_approvedMessageTypes.Any(mt => messageNamespace.Equals(mt, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new ArgumentException("Messagetype is not supported. Namespace:" + messageNamespace);
            }
        }
    }
}
