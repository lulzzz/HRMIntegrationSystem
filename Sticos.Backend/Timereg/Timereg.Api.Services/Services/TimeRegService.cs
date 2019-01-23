using System;
using Newtonsoft.Json;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Shared.Exceptions;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Integrations.Api.Contracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Shared.Interfaces;
using Timereg.Api.Domain.Validators.Interfaces;
using Timereg.Api.Services.Extensions;
using integrationsContracts = Integrations.Api.Contracts;

namespace Timereg.Api.Services.Services
{
    public class TimeRegService : ITimeRegService
    {
        private readonly IExternalSystemFactory _externalSystemFactory;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IIntegrationService _integrationService;
        private readonly IExternalSystemValidatorFactory _validatorFactory;
        private TimeregIntegrationValidator _integrationValidator;

        public ILogger<TimeRegService> Logger { get; set; }

        public TimeRegService(IExternalSystemFactory externalSystemFactory,
            IExternalSystemValidatorFactory validatorFactory,
            ICurrentUserContext currentUserContext,
            IIntegrationService integrationService)
        {
            _externalSystemFactory = externalSystemFactory;
            _validatorFactory = validatorFactory;
            _currentUserContext = currentUserContext;
            _integrationService = integrationService;
            _integrationValidator = new TimeregIntegrationValidator();

            Logger = NullLogger<TimeRegService>.Instance;
        }

        public async Task<HourBalance> GetHourBalance(int unitId, int employeeId)
        {
            var integration = await _integrationService.GetTimeregIntegration(unitId);
            var validationResult = await Validate(unitId,employeeId, integration);
           
            if (!validationResult.IsValid)
            {
                Logger.LogInformation($"{validationResult.Message}. HourBalance can not be fetched");
                if (validationResult.SkipFurtherProcessing)
                {   
                    return null;
                }
                throw new ForbiddenException(validationResult.Message);
            }

            var externalSystem = (ExternalEconomySystem)integration.ExternalSystem;
            var externalSystemAdapter = _externalSystemFactory.CreateSystemAdapter(externalSystem);
            return await externalSystemAdapter.GetHourBalance(unitId,employeeId);
        }

        private async Task<ITimeregValidationResult> Validate(int unitId, int employeeId, integrationsContracts.Integration integration)
        {
            var validationResult = _integrationValidator.Validate(integration);

            if (validationResult.SkipFurtherProcessing)
            {
                return validationResult;
            }
            else
            {
                var validatorFactory = _validatorFactory.Create(integration.ExternalSystem);
                validationResult = await validatorFactory.ValidateHourBalance(unitId, employeeId);
            }
            return validationResult;
        }
    }

}
