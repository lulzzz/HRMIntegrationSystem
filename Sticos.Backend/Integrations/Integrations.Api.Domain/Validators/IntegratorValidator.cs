using System;
using Integrations.Api.Domain.Validators.Interfaces;
using Integrations.Api.Domain.Validators.Models;
using Integrations.Api.Domain.Models;
using Integrations.Api.Domain.Validators.Extensions;
using Common.Api.Contracts.Services;

namespace Integrations.Api.Domain.Validators
{
    public class IntegratorValidator : IEntityValidator<Integration, int?>
    {
        private readonly IUnitService _unitService;

        public IntegratorValidator(IUnitService unitService)
        {
            _unitService = unitService;
        }

        public ValidatorResult ValidateCreate(Integration entity)
        {
            var  unit = _unitService.GetUnit(entity.UnitId).Result;

            var context = new ValidationContext<Integration>(entity);

            return context
                   .CommonValidateForWizzard(unit)
                   .Result;
        }

        public ValidatorResult ValidateDelete(int? id)
        {
            throw new NotImplementedException();
        }

        public ValidatorResult ValidateUpdate(Integration entity)
        {
            throw new NotImplementedException();
        }
    }
}
