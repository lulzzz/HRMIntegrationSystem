using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;

namespace Common.Api.Domain.Interfaces
{
    public interface IUnitService
    {
        Task<IEnumerable<Unit>> Search(SearchQueryUnit query);
        Task<Unit> GetUnit(int id);
    }
}