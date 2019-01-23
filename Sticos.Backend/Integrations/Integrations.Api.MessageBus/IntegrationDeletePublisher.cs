using MassTransit;
using Shared.MessageBus.Contracts;
using System.Threading.Tasks;

namespace Integrations.Api.MessageBus
{
    public class IntegrationDeletePublisher: IPublisher<IIntegrationDeleted>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public IntegrationDeletePublisher(
            IPublishEndpoint publishEndpoint
        ) {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Publish(object message)
        {
            await _publishEndpoint.Publish<IIntegrationDeleted>(message);
        }
    }
}
