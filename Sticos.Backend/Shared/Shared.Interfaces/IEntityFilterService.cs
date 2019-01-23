using Shared.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IEntityFilterService<T, TId> where T : EntityBase<T, TId>
    {
        Task<IEnumerable<TId>> GetIdFilter();
        Task<IQueryable<T>> GetBaseFilter();
    }
}
