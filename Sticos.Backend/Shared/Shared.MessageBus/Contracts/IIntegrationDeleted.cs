namespace Shared.MessageBus.Contracts
{
    public interface IIntegrationDeleted: IContract
    {
        int Id { get; set; }
        int UnitId { get; set; }
        int CustomerId { get; set; }
        int ExternalSystem { get; set; }
        int Category { get; set; }
    }
}
