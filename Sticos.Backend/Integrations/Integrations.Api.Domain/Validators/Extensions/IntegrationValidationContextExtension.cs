using Common.Api.Contracts;
using Integrations.Api.Domain.Models;
using Integrations.Api.Domain.Validators.Models;

namespace Integrations.Api.Domain.Validators.Extensions
{
    public static class IntegrationValidationContextExtension
    {
        public static ValidationContext<Integration> ValidateCompanyId(this ValidationContext<Integration> context)
        {
            if (context.StopValidate) return context;

            if (context.Model.UnitId == 0)
            {
                context.Result.AddErrorMessage("UnitId field is required", "UnitId");
                context.StopValidate = true;
            }

            return context;
        }

        public static ValidationContext<Integration> ValidationCategory(this ValidationContext<Integration> context)
        {
            if (context.StopValidate) return context;

            if (context.Model.Category == 0)
                context.Result.AddErrorMessage("Category field is required", "Category");

            return context;
        }

        public static ValidationContext<Integration> ValidationExternalSystem(this ValidationContext<Integration> context)
        {
            if (context.StopValidate) return context;

            if (context.Model.ExternalSystem == 0)
                context.Result.AddErrorMessage("ExternalSystem field is required", "ExternalSystem");

            return context;
        }

        public static ValidationContext<Integration> ValidateCompanyOrganizationNumber(this ValidationContext<Integration> context, Unit unit)
        {
            if (context.StopValidate) return context;

            if (string.IsNullOrWhiteSpace(unit?.BusinessOrganizationNumber))
                context.Result.AddErrorMessage("OrganizationNumber for company is missing, its required field");

            return context;
        }

        public static ValidationContext<Integration> ValidateIsNotNull(this ValidationContext<Integration> context)
        {
            if (context.StopValidate) return context;

            if (context.Model == null)
            {
                context.Result.AddErrorMessage("Invalid request.");
                context.StopValidate = true;
            }

            return context;
        }

        public static ValidationContext<Integration> CommonValidateForWizzard(this ValidationContext<Integration> context, Unit unit)
        {
            return context
                   .ValidateIsNotNull()
                   .ValidationCategory()
                   .ValidationExternalSystem()
                   .ValidateCompanyId()
                   .ValidateCompanyOrganizationNumber(unit);
        }
    }
}
