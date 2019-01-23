using Shared.Domain.ValueObjects.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Interfaces.Queries
{
    public interface IUnitQueries
    {
        Task<IEnumerable<UnitWithParent>> GetHierarchyUp(int fromUnitId);
        Task<IEnumerable<UnitWithParent>> GetHierarchyDown(int fromUnitId);
        Task<IEnumerable<UnitWithParent>> GetByIdList(IEnumerable<int> idList);
    }
}
