using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integrations.Api.Domain.Interfaces;
using Integrations.Api.Domain.Models;

namespace Integrations.Api.Domain.Services
{
    public class IntegrationCategoryService : IIntegrationCategoryService
    {
        private readonly IIntegrationCategoryRepository _integrationCategoryRepository;

        public IntegrationCategoryService(IIntegrationCategoryRepository integrationCategoryRepository)
        {
            _integrationCategoryRepository = integrationCategoryRepository;
        }
        public async Task<IEnumerable<IntegrationCategory>> GetCategories()
        {
            var categories = await _integrationCategoryRepository.GetCategories();
            return categories.OrderBy(c=>c.Order);
        }
    }
}
