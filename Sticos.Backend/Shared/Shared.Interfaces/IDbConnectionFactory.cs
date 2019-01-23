using System.Data;

namespace Shared.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetTenant();
        IDbConnection GetCore();

        string GetCoreDbName();
    }
}
