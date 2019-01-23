using Common.Api.Repositories.Legacy.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using System;
using System.Threading.Tasks;

namespace Common.Api.Repositories.Legacy.Factories
{
    public partial class PersonalLegacyContextFactory : IDbContextFactory<PersonalLegacyContext>
    {
        private const string CONNECTIONSTRING_KEY = "SticosPersonalLegacy";
        private readonly IConfiguration _configuration;
        private readonly ICustomerIdService _customerIdService;

        public PersonalLegacyContextFactory(IConfiguration configuration, ICustomerIdService customerIdService)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _customerIdService = customerIdService ?? throw new ArgumentNullException(nameof(customerIdService));
        }

        public Task<PersonalLegacyContext> CreateDbContext()
        {
            var connectionStringTemplate = _configuration.GetConnectionString(CONNECTIONSTRING_KEY);
            var connectionString = string.Format(connectionStringTemplate, _customerIdService.GetCustomerIdNotNull());

            var optionsBuilder = new DbContextOptionsBuilder<PersonalLegacyContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return Task.FromResult(new PersonalLegacyContext(optionsBuilder.Options));
        }
    }
}
