using System.Collections.Generic;
using System.Threading.Tasks;

namespace Integrations.Api.Contracts.Services
{
    public interface IIntegrationService
    {
        Task<Integration> GetIntegration(int id);
        Task<IEnumerable<Integration>> Search(SearchQueryIntegration searchQuery);
    }
}
