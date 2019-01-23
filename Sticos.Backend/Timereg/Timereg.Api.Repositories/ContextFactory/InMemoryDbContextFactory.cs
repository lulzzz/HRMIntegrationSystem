using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Threading.Tasks;
using Timereg.Api.Repositories.Context;

namespace Timereg.Api.Repositories.ContextFactory
{
    public class InMemoryDbContextFactory : IDbContextFactory<TimeregDbContext>
    {
        public Task<TimeregDbContext> CreateDbContext()
        {
            var db = "Timereg";

            var optionsBuilder = new DbContextOptionsBuilder<TimeregDbContext>();
            optionsBuilder.UseInMemoryDatabase(db);

            var context = new TimeregDbContext(optionsBuilder.Options);
            return Task.FromResult(context);
        }
    }
}
