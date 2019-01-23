using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace Shared.Services.Services
{
    public partial class InMemoryContextFactory<TContext> : IDbContextFactory<TContext>
        where TContext : DbContext, new()
    {

        public Task<TContext> CreateDbContext()
        {
            var dbName = typeof(TContext).Name;

            var optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseInMemoryDatabase(dbName);

            var ctx = (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);

            return Task.FromResult(ctx);
        }
    }
}
