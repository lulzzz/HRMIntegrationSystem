using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Api.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity, in TSearchQuery> : ISearchRepository<TEntity,TSearchQuery>
    {
        Task<TEntity> Create(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Delete(int id);
        Task<TEntity> GetById(int id);
        Task<bool> Exists(int id);
    }
    public interface ISearchRepository<TEntity, in TSearchQuery>
    {
        Task<IList<TEntity>> Search(TSearchQuery query);
    }
}