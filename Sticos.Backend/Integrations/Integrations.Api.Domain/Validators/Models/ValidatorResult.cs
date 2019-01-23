using System;
using System.Collections.Generic;
using System.Text;

namespace Integrations.Api.Domain.Validators.Models
{
    public class ValidatorResult
    {
        public ValidatorResult()
        {
            Errors = new List<string>();
        }

        public List<string> Errors { get; set; }
        public bool IsValid => Errors.Count == 0;

        public void AddErrorMessage(string message, string propertyName = null)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                Errors.Add(message);
            else
                Errors.Add($"{propertyName} - {message}");
        }
    }
}
