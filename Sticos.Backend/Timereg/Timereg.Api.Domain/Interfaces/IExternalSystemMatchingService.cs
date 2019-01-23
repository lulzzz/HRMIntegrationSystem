using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Contracts;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IExternalMatchingService
    {
        Task<IEnumerable<EntityMatch>> MatchEmployeeData(int unitId, int[] ids);
        Task<IEnumerable<EntityMatch>> MatchUnitData(int unitId, int[] ids);
        Task<IEnumerable<EntityMatch>> MatchAbsenceCodeData(int unitId, int[] ids);
    }
}
