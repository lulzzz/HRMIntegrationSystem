using Common.Api.Repositories.Legacy.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Threading.Tasks;


namespace Common.Api.Repositories.ContextFactory
{
    public class InMemoryPersonalLegacyContextFactory : IDbContextFactory<PersonalLegacyContext>
    {
        private ICustomerIdService _customerIdService;

        public InMemoryPersonalLegacyContextFactory(ICustomerIdService customerIdService)
        {
            _customerIdService = customerIdService;
        }

        public Task<PersonalLegacyContext> CreateDbContext()
        {
            var db = $"PersonalLegacy_{_customerIdService.GetCustomerIdNotNull()}";

            var optionsBuilder = new DbContextOptionsBuilder<PersonalLegacyContext>();
            optionsBuilder.UseInMemoryDatabase(db);

            return Task.FromResult(new PersonalLegacyContext(optionsBuilder.Options));
        }
    }
}
