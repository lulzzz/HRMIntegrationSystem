using Common.Api.Repositories.Legacy.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Threading.Tasks;

namespace Common.Api.Repositories.ContextFactory
{
    public class InMemoryPersonalCommonLegacyContextFactory : IDbContextFactory<PersonalCommonLegacyContext>
    {
        public InMemoryPersonalCommonLegacyContextFactory()
        {
        }

        public async Task<PersonalCommonLegacyContext> CreateDbContext()
        {
            var db = $"PersonalCommonLegacy";

            var optionsBuilder = new DbContextOptionsBuilder<PersonalCommonLegacyContext>();
            optionsBuilder.UseInMemoryDatabase(db);

            var context = new PersonalCommonLegacyContext(optionsBuilder.Options);
            await Seed(context);
            return context;
        }

        private async Task Seed(PersonalCommonLegacyContext context)
        {
        }
    }
}
