using Timereg.Api.Domain.AdaptersValidators.Interfaces;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IExternalSystemValidatorFactory
    {
        IValidateAdapter Create(int externalSystemId);
    }
}
