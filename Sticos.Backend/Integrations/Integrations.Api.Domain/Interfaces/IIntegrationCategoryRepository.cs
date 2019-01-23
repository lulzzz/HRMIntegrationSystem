using System.Collections.Generic;
using System.Threading.Tasks;
using Integrations.Api.Domain.Models;

namespace Integrations.Api.Domain.Interfaces
{
    public interface IIntegrationCategoryRepository
    {
        Task<List<IntegrationCategory>> GetCategories();
    }
}
