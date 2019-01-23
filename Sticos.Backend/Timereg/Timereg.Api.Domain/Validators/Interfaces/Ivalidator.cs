using Integrations.Api.Contracts;
using Timereg.Api.Domain.Validators.Models;

namespace Timereg.Api.Domain.Validators.Interfaces
{
    public class TimeregIntegrationValidator
    {
        public ITimeregValidationResult Validate(Integration integration)
        {
            if (integration == null)
            {
                return new FailedResult("A Timereg integration is not found") {SkipFurtherProcessing = true};
            }
            else if (!integration.IsActivated)
            {
                return new FailedResult("The Timereg integration is not active.");
            }

            return new OkResult("Integration is found and active");
        }
    }
}