using System;

namespace Timereg.Api.Domain.Exceptions
{
    public class ExternalSystemCommunicationException : Exception
    {
        public ExternalSystemCommunicationException(string message) : base(message) {}
        public ExternalSystemCommunicationException(string message, Exception innerException) : base(message,innerException) {}
    }
}