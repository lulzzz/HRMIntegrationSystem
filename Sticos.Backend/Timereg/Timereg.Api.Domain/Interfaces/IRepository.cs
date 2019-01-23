using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Timereg.Api.Domain.Interfaces
{
    public interface IRepository<TEntity, TSearchQuery>
    {
        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> GetSingle(int Id);
        Task<IEnumerable<TEntity>> Search(TSearchQuery searchQuery);
    }
}
