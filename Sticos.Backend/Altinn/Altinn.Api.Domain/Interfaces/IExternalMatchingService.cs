using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Contracts;

namespace Altinn.Api.Domain.Interfaces
{
    public interface IExternalMatchingService
    {
        Task<IEnumerable<EntityMatch>> MatchUnitData(int unitId, int[] ids);
    }
}
