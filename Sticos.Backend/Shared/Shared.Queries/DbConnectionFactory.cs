using Shared.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Shared.Queries
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public DbConnectionFactory(
            IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public IDbConnection GetTenant()
        {
            var connectionString = _connectionStringProvider.GetForCustomer();

            return new SqlConnection(connectionString);
        }

        public IDbConnection GetCore()
        {
            var connectionString = _connectionStringProvider.GetForShared();

            return new SqlConnection(connectionString);
        }

        public string GetCoreDbName()
        {
            return "SticosPersonalFelles";
        }
    }
}
