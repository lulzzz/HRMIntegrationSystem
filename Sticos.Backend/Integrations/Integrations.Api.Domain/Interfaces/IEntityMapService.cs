using System.Collections.Generic;
using System.Threading.Tasks;
using Integrations.Api.Domain.Models;

namespace Integrations.Api.Domain.Interfaces
{
    public interface IEntityMapService
    {
        Task<IEnumerable<EntityMap>> Search(SearchQueryEntityMap searchQuery);
        Task<IEnumerable<EntityMap>> UpdateEntityMaps(IEnumerable<EntityMap> entityMaps);
        Task DeleteEmployee(EmployeeDeleted message);
    }
}
