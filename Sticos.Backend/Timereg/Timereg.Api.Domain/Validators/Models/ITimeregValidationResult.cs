using Timereg.Api.Domain.Validators.Interfaces;

namespace Timereg.Api.Domain.Validators.Models
{
    public class FailedResult : ITimeregValidationResult
    {
        public FailedResult(string failedMessage)
        {
            Message = failedMessage;
        }
        public string Message { get; }
        public bool IsValid { get; } = false;
        public bool SkipFurtherProcessing { get; set; }
    }
   
    public class OkResult : ITimeregValidationResult
    {
        public OkResult(string okMessage)
        {
            Message = okMessage;
            SkipFurtherProcessing = false;
        }
        public string Message { get;  }
        public bool IsValid { get; } = true;
        public bool SkipFurtherProcessing { get; }
    }
}