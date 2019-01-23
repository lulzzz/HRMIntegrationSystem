using System;
using System.Collections.Generic;
using System.Text;
using Timereg.Api.Domain.Validators.Models;

namespace Timereg.Api.Domain.Validators.Interfaces
{
    public interface IEntityValidator<T,Tkey> where T : class
    {
        ValidatorResult ValidateCreate(T entity);
        ValidatorResult ValidateUpdate(T entity);
        ValidatorResult ValidateDelete(Tkey id);
    }
}
