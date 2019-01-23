using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Api.Contracts.Services
{
    public interface IUnitService
    {
        Task<IEnumerable<Unit>> SearchUnits(SearchQueryUnit searchQuery);
        Task<Unit> GetUnit(int id);
    }
}