using Microsoft.EntityFrameworkCore;

namespace Integrations.Api.Repositories.Context
{
    public class IntegrationDbContext : DbContext
    {
        public IntegrationDbContext(DbContextOptions<IntegrationDbContext> options) : base(options)
        {
        }
        public DbSet<Models.EntityMap> EntityMaps { get; set; }
        public DbSet<Models.Integration> Integrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Integrations");
        }
    }
}