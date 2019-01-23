using Altinn.Api.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Threading.Tasks;

namespace Altinn.Api.Repositories.ContextFactory
{
    public class InMemoryDbContextFactory : IDbContextFactory<AltinnDbContext>
    {
        public Task<AltinnDbContext> CreateDbContext()
        {
            var database = "Altinn";

            var optionsBuilder = new DbContextOptionsBuilder<AltinnDbContext>();
            optionsBuilder.UseInMemoryDatabase(database);

            var context = new AltinnDbContext(optionsBuilder.Options);

            return Task.FromResult(context);
        }
    }
}
