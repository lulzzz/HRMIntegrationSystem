using Common.Api.Domain.Entities;
using Common.Api.Domain.Validators.Extensions;
using Common.Api.Domain.Validators.Interfaces;
using Common.Api.Domain.Validators.Models;

namespace Common.Api.Domain.Validators
{
    public class DashboardValidator : IEntityValidator<Dashboard, int?>
    {
        public ValidatorResult ValidateCreate(Dashboard entity)
        {
            var context = new ValidationContext<Dashboard>(entity);
            return context
                .CommonValidation()
                .ValidateIdNotAllowed()
                .Result;
        }

        public ValidatorResult ValidateDelete(int? id)
        {
            var context = new ValidationContext<int?>(id);
            return context
                .ValidateIsNotNull("Id")
                .ValidateGreaterThan(0, "Id")
                .Result;
        }

        public ValidatorResult ValidateUpdate(Dashboard entity)
        {
            var context = new ValidationContext<Dashboard>(entity);
            return context
                .CommonValidation()
                .ValidateIdProvided()
                .Result;
        }
    }
}