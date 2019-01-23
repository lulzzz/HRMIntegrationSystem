using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace Shared.Services.Services
{
    public partial class DbContextFactory<TContext> : IDbContextFactory<TContext>
        where TContext : DbContext, new()
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public DbContextFactory(
            IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider ?? throw new ArgumentNullException(nameof(connectionStringProvider));
        }

        public Task<TContext> CreateDbContext()
        {
            var connectionString = _connectionStringProvider.GetForCustomer();

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var ctx = (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);

            return Task.FromResult(ctx);
        }
    }
}
