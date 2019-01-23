using System;
using System.Collections.Generic;
using System.Text;

namespace Timereg.Api.Domain.Validators.Models
{
    public class ValidationContext<T>
    {
        public ValidationContext(T model)
        {
            Result = new ValidatorResult();
            Model = model;
        }

        public bool StopValidate { get; set; }
        public T Model { get; set; }
        public ValidatorResult Result { get; set; }
}
}
