using System;
using Integrations.Api.Contracts.Services;
using Shared.Interfaces;
using Timereg.Api.Domain.AdaptersValidators.Interfaces;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Timereg.Api.Unimicro.Validators;

namespace Timereg.Api.Services.Services
{
    public class ExternalSystemValidatorFactory : IExternalSystemValidatorFactory
    {
        private readonly IIntegrationService _integrationService;
        private readonly IEntityMapService _entityMapService;

        public ExternalSystemValidatorFactory(IIntegrationService integrationService, IEntityMapService entityMapService)
        {
            _integrationService = integrationService;
            _entityMapService = entityMapService;
        }

        public IValidateAdapter Create(int externalSystemId)
        {
            var externalSystem = (ExternalEconomySystem)externalSystemId;

            switch (externalSystem)
            {
                case ExternalEconomySystem.UniMicro:
                    return new UnimicroValidator(_integrationService, _entityMapService);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
