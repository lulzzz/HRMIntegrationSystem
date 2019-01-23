using System.Threading.Tasks;
using Timereg.Api.Domain.Models;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IExternalSystemFactory
    {
        IExternalDataService CreateDataService(ExternalEconomySystem externalSystem);
        IExternalSystemAdapter CreateSystemAdapter(ExternalEconomySystem externalSystem);
        IExternalMatchingService CreateMatchingService(ExternalEconomySystem externalSystem);
    }
}
