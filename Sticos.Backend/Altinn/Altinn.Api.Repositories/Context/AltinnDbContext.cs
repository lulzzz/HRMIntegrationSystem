using Microsoft.EntityFrameworkCore;
using Altinn.Api.Repositories.Models;

namespace Altinn.Api.Repositories.Context
{
    public class AltinnDbContext : DbContext
    {
        public DbSet<NavMessage> NavMessages { get; set; }

        public AltinnDbContext(DbContextOptions<AltinnDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Altinn");
        }
    }
}
