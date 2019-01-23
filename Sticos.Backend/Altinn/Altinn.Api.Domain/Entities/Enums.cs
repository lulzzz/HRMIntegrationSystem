namespace Altinn.Api.Domain.Entities
{
    public enum IntegrationType
    {
        Unknown = 0,
        Import = 1,
        Export = 2,
    }
    public enum WorkState
    {
        Unknown = 0,
        ReadyForProcessing = 1,
        ReadyforExport = 5,
        CompletedSuccessfully = 10,
        CompletedWithFailure = 15,
    }

    public enum ExternalGovernmentSystem
    {
        Unknown = 0,
        DigiSyfo = 1,
    }
}