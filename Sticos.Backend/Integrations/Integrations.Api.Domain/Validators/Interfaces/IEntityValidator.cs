
using Integrations.Api.Domain.Validators.Models;

namespace Integrations.Api.Domain.Validators.Interfaces
{
    public interface IEntityValidator<T,Tkey> where T : class
    {
        ValidatorResult ValidateCreate(T entity);
        ValidatorResult ValidateUpdate(T entity);
        ValidatorResult ValidateDelete(Tkey id);
    }
}
