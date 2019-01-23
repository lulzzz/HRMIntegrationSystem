using Common.Api.Domain.Validators.Models;

namespace Common.Api.Domain.Validators.Extensions
{
    public static class NullableIntValidationContextExtensions
    {
        public static ValidationContext<int?> ValidateIsNotNull(this ValidationContext<int?> context,
            string propertyName)
        {
            if (context.StopValidate) return context;

            if (!context.Model.HasValue)
            {
                context.Result.AddErrorMessage($"{propertyName} is missing.", propertyName);
                context.StopValidate = true;
            }

            return context;
        }

        public static ValidationContext<int?> ValidateGreaterThan(this ValidationContext<int?> context, int value,
            string propertyName)
        {
            if (context.StopValidate) return context;

            // ReSharper disable once PossibleInvalidOperationException
            if (context.Model.Value <= value)
                context.Result.AddErrorMessage($"{propertyName} should be greater than {value}.", propertyName);

            return context;
        }
    }
}