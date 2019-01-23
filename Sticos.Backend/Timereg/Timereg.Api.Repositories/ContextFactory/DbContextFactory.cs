using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using System.Threading.Tasks;
using Timereg.Api.Repositories.Context;

namespace Timereg.Api.Repositories.ContextFactory
{
    public class DbContextFactory : IDbContextFactory<TimeregDbContext>,
                                    IDesignTimeDbContextFactory<TimeregDbContext>
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IConfiguration _configuration;
        private readonly ICustomerIdService _customerIdService;

        public DbContextFactory(IConfiguration configuration, ICustomerIdService customerIdService)
        {
            _configuration = configuration;
            _customerIdService = customerIdService;
        }

        // Needed empty contructor for DesignTime pattern
        public DbContextFactory() { }

        private IConfiguration Configuration { get; }

        public async Task<TimeregDbContext> CreateDbContext()
        {
            var connectionString = string.Format(_configuration.GetConnectionString("Default"), _customerIdService.GetCustomerIdNotNull());

            var optionsBuilder = new DbContextOptionsBuilder<TimeregDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new TimeregDbContext(optionsBuilder.Options);
            context.Database.Migrate();
            return context;
        }

        public TimeregDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=localhost; Database=Timereg_x; Trusted_Connection=True";
            var optionsBuilder = new DbContextOptionsBuilder<TimeregDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            var context = new TimeregDbContext(optionsBuilder.Options);
            return context;
        }
    }
}
