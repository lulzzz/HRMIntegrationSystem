using Shared.Domain;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IEntityAuthorizationService<TEntity, TId> where TEntity : EntityBase<TEntity, TId>
    {
        Task<bool> CanCreate(TEntity entity);
        Task<bool> CanUpdate(TEntity entity);
        Task<bool> CanDelete(TEntity entity);
    }
}
