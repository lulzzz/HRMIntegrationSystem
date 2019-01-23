using System.Collections.Generic;
using System.Threading.Tasks;

namespace Altinn.Api.Domain.Interfaces
{
    public interface IRepository<TEntity, TSearchQuery>
    {
        Task<TEntity> Save(TEntity entity);
        Task<TEntity> Get(int id);
        Task<IEnumerable<TEntity>> Search(TSearchQuery searchQuery);
    }

    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> Search();
    }
}