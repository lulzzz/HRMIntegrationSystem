using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;

namespace Timereg.Api.UnitTests.Builders
{
    public class IntegrationServiceBuilder
    {
        private IList<Integrations.Api.Contracts.Integration> _integrations = new List<Integrations.Api.Contracts.Integration>();
        private IIntegrationService _integrationService = A.Fake<IIntegrationService>();
        
        public IntegrationServiceBuilder WithIntegration(Integrations.Api.Contracts.Integration integration)
        {
            _integrations.Add(integration);
            return this;
        }

        public IIntegrationService Build()
        {
            A.CallTo(() => _integrationService.GetIntegration(A<int>._))
                .Invokes(id => _integrations.FirstOrDefault(integrations => integrations.Id == id.GetArgument<int>(0)));
            return _integrationService;
        }
    }
}