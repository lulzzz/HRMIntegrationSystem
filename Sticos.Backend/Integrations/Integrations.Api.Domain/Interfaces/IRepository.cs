using Integrations.Api.Domain.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrations.Api.Domain.Interfaces
{
    public interface IEntityMapRepository
    {
        Task<IList<EntityMap>> Search(SearchQueryEntityMap searchQuery);
        Task RemoveAllMatching(IEnumerable<EntityMap> entities);
        Task<IList<EntityMap>> Add(IEnumerable<EntityMap> entities);
        Task DeleteEmployee(EmployeeDeleted message);
    }

    public interface IRepository<TEntity, TSearchQuery>
    {
        Task<TEntity> Create(TEntity entity);
        Task<IList<TEntity>> CreateAll(IEnumerable<TEntity> entities);
        Task<IList<TEntity>> UpdateAll(IEnumerable<TEntity> entities);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> GetSingle(int id);
        Task<IList<TEntity>> Search(TSearchQuery searchQuery);
        Task<TEntity> Delete(int id);
    }
}
