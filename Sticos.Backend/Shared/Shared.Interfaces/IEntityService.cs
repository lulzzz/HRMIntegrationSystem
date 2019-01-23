using System.Linq;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IEntityService<T, TId, TContext>
    {
        Task<IQueryable<T>> GetQuery();
        Task<T> GetById(TId id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(TId id);
    }
}
