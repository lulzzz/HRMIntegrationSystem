using Altinn.Api.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace Altinn.Api.Repositories.ContextFactory
{
    public class DbContextFactory : IDbContextFactory<AltinnDbContext>,
                                   IDesignTimeDbContextFactory<AltinnDbContext>
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly ICustomerIdService _customerIdService;

        public DbContextFactory(IConfiguration configuration, ICustomerIdService customerIdService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _customerIdService = customerIdService ?? throw new ArgumentNullException(nameof(customerIdService));
        }

        public DbContextFactory() { }

        public async Task<AltinnDbContext> CreateDbContext()
        {
            var connectionString = string.Format(_configuration.GetConnectionString("Default"), _customerIdService.GetCustomerIdNotNull());

            var optionsBuilder = new DbContextOptionsBuilder<AltinnDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new AltinnDbContext(optionsBuilder.Options);
            await context.Database.MigrateAsync();

            return context;
        }

        public AltinnDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost; Database=AltinnDB_x; Trusted_Connection=True";
            var optionsBuilder = new DbContextOptionsBuilder<AltinnDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new AltinnDbContext(optionsBuilder.Options);
            return context;
        }
    }
}
