using Common.Api.Domain.Entities;
using Common.Api.Domain.Validators.Models;

namespace Common.Api.Domain.Validators.Extensions
{
    public static class DashboardValidationContextExtensions
    {
        public static ValidationContext<Dashboard> ValidateIsNotNull(this ValidationContext<Dashboard> context)
        {
            if (context.StopValidate) return context;

            if (context.Model == null)
            {
                context.Result.AddErrorMessage("Invalid request.");
                context.StopValidate = true;
            }

            return context;
        }

        public static ValidationContext<Dashboard> ValidateDashboardConfig(this ValidationContext<Dashboard> context)
        {
            if (context.StopValidate) return context;

            if (string.IsNullOrWhiteSpace(context.Model.DashboardConfig))
                context.Result.AddErrorMessage("DashboardConfig is required.", "DashboardConfig");

            return context;
        }

        public static ValidationContext<Dashboard> ValidateTitle(this ValidationContext<Dashboard> context)
        {
            if (context.StopValidate) return context;

            if (string.IsNullOrWhiteSpace(context.Model.Title))
            {
                context.Result.AddErrorMessage("Title is required.", "Title");
            }
            else
            {
                if (context.Model.Title.Length > 255)
                    context.Result.AddErrorMessage("Title is limited to maximum of 255 characters.", "Title");
            }

            return context;
        }

        public static ValidationContext<Dashboard> ValidateOwnerTypeId(this ValidationContext<Dashboard> context)
        {
            if (context.StopValidate) return context;

            if (context.Model.OwnerTypeId <= 0)
                context.Result.AddErrorMessage("OwnerTypeId is required.", "OwnerTypeId");

            return context;
        }

        public static ValidationContext<Dashboard> ValidateIdNotAllowed(this ValidationContext<Dashboard> context)
        {
            if (context.StopValidate) return context;

            if (context.Model.Id.HasValue) context.Result.AddErrorMessage("Id not allowed.", "Id");

            return context;
        }

        public static ValidationContext<Dashboard> ValidateIdProvided(this ValidationContext<Dashboard> context)
        {
            if (context.StopValidate) return context;

            if (!context.Model.Id.HasValue) context.Result.AddErrorMessage("Id is missing.", "Id");

            return context;
        }

        public static ValidationContext<Dashboard> CommonValidation(this ValidationContext<Dashboard> context)
        {
            return context
                .ValidateIsNotNull()
                .ValidateDashboardConfig()
                .ValidateTitle()
                .ValidateOwnerTypeId();
        }
    }
}