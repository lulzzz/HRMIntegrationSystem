using System;
using System.Collections.Generic;

namespace Shared.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(List<string> errors)
        {
            Errors = errors;
        }

        public List<string> Errors { get; } = new List<string>();
    }
}