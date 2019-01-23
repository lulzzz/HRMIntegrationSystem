using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrations.Api.Contracts.Services
{
    public interface IEntityMapService
    {
        Task<IEnumerable<EntityMap>> SearchEntityMaps(SearchQueryEntityMap searchQuery);
    }
}
