namespace Common.Api.Domain.Validators.Models
{
    public class ValidationContext<T>
    {
        public ValidationContext(T model)
        {
            Result = new ValidatorResult();
            Model = model;
        }

        public bool StopValidate { get; set; } = false;
        public T Model { get; set; }
        public ValidatorResult Result { get; set; }
    }
}