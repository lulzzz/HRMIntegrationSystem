using Common.Api.Repositories.Legacy.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.Interfaces;
using System;
using System.Threading.Tasks;


namespace Common.Api.Repositories.Legacy.Factories
{
    public partial class PersonalCommonLegacyContextFactory : IDbContextFactory<PersonalCommonLegacyContext>
    {
        private const string CONNECTIONSTRING_KEY = "SticosPersonalCommonLegacy";
        private readonly IConfiguration _configuration;

        public PersonalCommonLegacyContextFactory(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public Task<PersonalCommonLegacyContext> CreateDbContext()
        {
            var connectionString = _configuration.GetConnectionString(CONNECTIONSTRING_KEY);

            var optionsBuilder = new DbContextOptionsBuilder<PersonalCommonLegacyContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return Task.FromResult(new PersonalCommonLegacyContext(optionsBuilder.Options));
        }
    }
}
