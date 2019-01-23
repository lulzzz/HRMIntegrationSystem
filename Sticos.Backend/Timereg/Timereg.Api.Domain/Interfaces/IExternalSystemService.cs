using System.Collections.Generic;
using System.Threading.Tasks;
using Timereg.Api.Domain.Models;
using Shared.Contracts;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IExternalSystemService
    {
        Task<IEnumerable<ExternalSystem>> Search(SearchQueryExternalSystem query);
        Task<IEnumerable<ExternalData>> GetExternalData(int externalSystem, int unitId, int entity);
        Task<IEnumerable<EntityMatch>> MatchEntities(int externalSystem,int unitId, int entity, int[] ids);
        Task<ExternalSystem> GetExternalSystem(int externalSystem);
    }
}
