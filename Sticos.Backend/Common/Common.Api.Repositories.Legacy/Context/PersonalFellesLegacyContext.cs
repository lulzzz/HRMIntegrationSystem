using Common.Api.Repositories.Legacy.Models;
using Microsoft.EntityFrameworkCore;

namespace Common.Api.Repositories.Legacy.Context
{
    public sealed class PersonalCommonLegacyContext : DbContext
    {
        public PersonalCommonLegacyContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}