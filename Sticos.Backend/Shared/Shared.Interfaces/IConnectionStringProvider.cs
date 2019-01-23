namespace Shared.Interfaces
{
    public interface IConnectionStringProvider
    {
        string GetForCustomer();
        string GetForShared();
    }
}
