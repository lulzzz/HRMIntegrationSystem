using Common.Api.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Api.Repositories.Context
{
    public class SticosWidgetDbContext : DbContext
    {
        public SticosWidgetDbContext(DbContextOptions<SticosWidgetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<OwnerType> OwnerTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Common");
        }
    }
}