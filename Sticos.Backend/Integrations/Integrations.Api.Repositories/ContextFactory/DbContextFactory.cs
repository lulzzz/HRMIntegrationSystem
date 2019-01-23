
using Integrations.Api.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace Integrations.Api.Repositories.ContextFactory
{
    public class DbContextFactory : IDbContextFactory<IntegrationDbContext>,
                                    IDesignTimeDbContextFactory<IntegrationDbContext>
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


        public async Task<IntegrationDbContext> CreateDbContext()
        {
            var connectionString = string.Format(_configuration.GetConnectionString("Default"), _customerIdService.GetCustomerIdNotNull());

            var optionsBuilder = new DbContextOptionsBuilder<IntegrationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new IntegrationDbContext(optionsBuilder.Options);
            await context.Database.MigrateAsync();
            return context;
        }

        public IntegrationDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost; Database=Timereg_x; Trusted_Connection=True";
            var optionsBuilder = new DbContextOptionsBuilder<IntegrationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new IntegrationDbContext(optionsBuilder.Options);
            return context;
        }
    }
}
