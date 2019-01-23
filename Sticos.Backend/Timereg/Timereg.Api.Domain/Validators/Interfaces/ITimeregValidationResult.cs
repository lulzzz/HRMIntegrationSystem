namespace Timereg.Api.Domain.Validators.Interfaces
{
    public interface ITimeregValidationResult
    {
        string Message { get; }
        bool IsValid { get;}
        bool SkipFurtherProcessing { get; }
    }
}