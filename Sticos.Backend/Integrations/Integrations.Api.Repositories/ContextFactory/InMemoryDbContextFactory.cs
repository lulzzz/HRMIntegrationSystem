using Integrations.Api.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Threading.Tasks;


namespace Integrations.Api.Repositories.ContextFactory
{
    public class InMemoryDbContextFactory : IDbContextFactory<IntegrationDbContext>
    {
        public Task<IntegrationDbContext> CreateDbContext()
        {
            var db = "Timereg";

            var optionsBuilder = new DbContextOptionsBuilder<IntegrationDbContext>();
            optionsBuilder.UseInMemoryDatabase(db);

            var context = new IntegrationDbContext(optionsBuilder.Options);
            return Task.FromResult(context);
        }
    }
}
