using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Contracts;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IExternalDataService
    {
        Task<IEnumerable<ExternalData>> GetExternalEmployeeData(int unitId);
        Task<IEnumerable<ExternalData>> GetExternalUnitData(int unitId);
        Task<IEnumerable<ExternalData>> GetExternalAbsenceCodeData(int unitId);
    }
}
