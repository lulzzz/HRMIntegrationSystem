using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IDbContextFactory<T> where T : DbContext
    {
        Task<T> CreateDbContext();
    }
}
