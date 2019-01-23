using Common.Api.Domain.Entities;
using System.Threading.Tasks;

namespace Common.Api.Domain.Interfaces.Repositories
{
    public interface IUnitRepository : ISearchRepository<Unit,SearchQueryUnit>
    {
       Task<Unit> GetUnit(int id);
    }
}