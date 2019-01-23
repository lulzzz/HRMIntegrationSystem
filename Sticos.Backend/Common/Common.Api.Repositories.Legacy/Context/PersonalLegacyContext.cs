using Common.Api.Repositories.Legacy.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Api.Repositories.Legacy.Context
{
    public sealed class PersonalLegacyContext : DbContext
    {
        public PersonalLegacyContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Unit> Units { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<UnitContentBook> UnitContentBooks { get; set; }
        public DbSet<OrgUnitVerification> OrgUnitVerifications { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Employment> Employments { get; set; }
        public DbSet<EmployeePermission> EmployeePermissions { get; set; }
    }
}