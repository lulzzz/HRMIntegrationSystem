using System.Collections;
using FakeItEasy;
using Integrations.Api.Contracts.Services;
using Shared.Interfaces;
using Timereg.Api.Domain.AdaptersValidators.Interfaces;
using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Services.Services;

namespace Timereg.Api.UnitTests.Builders
{
    public class TimeRegServiceBuilder
    {
        private IExternalSystemFactory _externalSystemFactory = A.Fake<IExternalSystemFactory>();
        private ICurrentUserContext _currentUserContext = A.Fake<ICurrentUserContext>();
        private IIntegrationService _integrationService = A.Fake<IIntegrationService>();
        private IExternalSystemValidatorFactory _validatorFactory = A.Fake<IExternalSystemValidatorFactory>();
        private IValidateAdapter _validator = A.Fake<IValidateAdapter>();

        public TimeRegServiceBuilder WithExternalSystemFactory(IExternalSystemFactory externalSystemFactory)
        {
            _externalSystemFactory = externalSystemFactory;
            return this;
        }
        public TimeRegServiceBuilder WithCurrentUserContext(ICurrentUserContext currentUserContext)
        {
            _currentUserContext = currentUserContext;
            return this;
        }
        public TimeRegServiceBuilder WithIntegrationService(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
            return this;
        }   
        public TimeRegServiceBuilder WithValidator(IValidateAdapter validator)
        {
            _validator = validator;
            return this;
        }

        public ITimeRegService Build()
        {
            A.CallTo(() => _validatorFactory.Create(A<int>.Ignored)).Returns(_validator);
            return new TimeRegService(_externalSystemFactory, _validatorFactory,_currentUserContext, _integrationService);
        }
    }
}