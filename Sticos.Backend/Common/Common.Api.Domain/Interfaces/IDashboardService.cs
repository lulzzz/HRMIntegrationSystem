using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Api.Domain.Entities;

namespace Common.Api.Domain.Interfaces
{
    public interface IDashboardService
    {
        Task<Dashboard> Create(Dashboard entity);
        Task<Dashboard> Update(Dashboard entity);
        Task<Dashboard> Delete(int id);
        Task<Dashboard> GetById(int id);
        Task<IEnumerable<Dashboard>> Search(SearchQueryDashboard query);
    }
}