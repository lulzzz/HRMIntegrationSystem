using Microsoft.EntityFrameworkCore;
using Timereg.Api.Repositories.Models;

namespace Timereg.Api.Repositories.Context
{
    public class TimeregDbContext : DbContext
    {
        public TimeregDbContext(DbContextOptions<TimeregDbContext> options)
               : base(options)
        {
        }

        public DbSet<AbsenceExport> AbsenceExports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Timereg");
        }
    }
}