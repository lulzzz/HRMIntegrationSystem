using Common.Api.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace Common.Api.Repositories.ContextFactory
{
    public class DbContextFactory : IDbContextFactory<SticosWidgetDbContext>,
                                    IDesignTimeDbContextFactory<SticosWidgetDbContext>
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerIdService _customerIdService;

        public DbContextFactory(IConfiguration configuration, ICustomerIdService customerIdService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _customerIdService = customerIdService ?? throw new ArgumentNullException(nameof(customerIdService));
        }

        // Needed empty contructor for DesignTime pattern
        public DbContextFactory() { }

        private IConfiguration Configuration { get; }

        public async Task<SticosWidgetDbContext> CreateDbContext()
        {
            var connectionString = string.Format(_configuration.GetConnectionString("Default"), _customerIdService.GetCustomerIdNotNull());

            var optionsBuilder = new DbContextOptionsBuilder<SticosWidgetDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new SticosWidgetDbContext(optionsBuilder.Options);
            await context.Database.MigrateAsync();
            return context;
        }

        public SticosWidgetDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost; Database=SticosWidget_x; Trusted_Connection=True";
            var optionsBuilder = new DbContextOptionsBuilder<SticosWidgetDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new SticosWidgetDbContext(optionsBuilder.Options);
            return context;
        }
    }
}
