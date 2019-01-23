using Integrations.Api.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrations.Api.Domain.Interfaces
{
    public interface IIntegrationService
    {
        Task<Integration> CreateIntegration(Integration integration);
        Task<Integration> GetIntegration(int id);
        Task<IEnumerable<Integration>> Search(SearchQueryIntegration searchQuery);
        Task<Integration> UpdateIntegration(Integration integration);
        Task Delete(int id);
    }
}
