using Common.Api.Repositories.Context;
using Common.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Threading.Tasks;

namespace Common.Api.Repositories.ContextFactory
{
    public class InMemorySticosWidgetDbContextFactory : IDbContextFactory<SticosWidgetDbContext>
    {
        private readonly ICustomerIdService _customerIdService;

        public InMemorySticosWidgetDbContextFactory(ICustomerIdService customerIdService)
        {
            _customerIdService = customerIdService;
        }

        public async Task<SticosWidgetDbContext> CreateDbContext()
        {
            var db = $"SticosWidgets_{_customerIdService.GetCustomerIdNotNull()}";

            var optionsBuilder = new DbContextOptionsBuilder<SticosWidgetDbContext>();
            optionsBuilder.UseInMemoryDatabase(db);

            var context = new SticosWidgetDbContext(optionsBuilder.Options);
            await Seed(context);
            return context;
        }

        private async Task Seed(SticosWidgetDbContext context)
        {
            if (await context.OwnerTypes.CountAsync() == 0)
            {
                context.Add(new OwnerType { Name = "Default", Priority = 3 });
                context.Add(new OwnerType { Name = "Company", Priority = 2 });
                context.Add(new OwnerType { Name = "User", Priority = 1 });
                context.SaveChanges();
            }
        }
    }
}
