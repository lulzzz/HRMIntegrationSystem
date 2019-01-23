using Common.Api.Domain.Validators.Models;

namespace Common.Api.Domain.Validators.Interfaces
{
    public interface IEntityValidator<T, Tkey> where T : class
    {
        ValidatorResult ValidateCreate(T entity);
        ValidatorResult ValidateUpdate(T entity);
        ValidatorResult ValidateDelete(Tkey id);
    }
}