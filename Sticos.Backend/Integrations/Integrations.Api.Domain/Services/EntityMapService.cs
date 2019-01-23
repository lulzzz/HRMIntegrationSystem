using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Integrations.Api.Domain.Interfaces;
using Integrations.Api.Domain.Models;
using Shared.Interfaces.Queries;

namespace Integrations.Api.Domain.Services
{
    public class EntityMapService : IEntityMapService
    {
        private readonly IEntityMapRepository _repository;
        private readonly IIntegrationService _integrationService;
        private readonly IUnitQueries _unitQueries;

        public EntityMapService(IEntityMapRepository repository, IIntegrationService integrationService, IUnitQueries unitQueries)
        {
            _repository = repository;
            _integrationService = integrationService;
            _unitQueries = unitQueries;
        }

        public async Task DeleteEmployee(EmployeeDeleted message)
        {
            await _repository.DeleteEmployee(message);
        }

        public async Task<IEnumerable<EntityMap>> Search(SearchQueryEntityMap searchQuery)
        {
            if (searchQuery.IntegrationId <= 0)
            {
                var integrationId = await SearchForIntegrationId(searchQuery);
                searchQuery.IntegrationId = integrationId ?? 0;
            }

            return await _repository.Search(searchQuery);

        }

        private async Task<int?> SearchForIntegrationId(SearchQueryEntityMap searchQuery)
        {
            int? integrationId = null;
            if (searchQuery.UnitId > 0 && searchQuery.ExternalSystemId > 0 && searchQuery.IntegrationCategory > 0)
            {
                var integrationQuery = new SearchQueryIntegration(searchQuery.UnitId, searchQuery.IntegrationCategory, searchQuery.ExternalSystemId);
                var unitIds = await _unitQueries.GetHierarchyUp(searchQuery.UnitId);

                integrationQuery.UnitIds = unitIds.Select(u=>u.Id).ToList();
                var integrations = await _integrationService.Search(integrationQuery);
                integrationId = integrations?.FirstOrDefault()?.Id;
            }

            return integrationId;
        }

        public async Task<IEnumerable<EntityMap>> UpdateEntityMaps(IEnumerable<EntityMap> entityMaps)
        {
            await _repository.RemoveAllMatching(entityMaps);
            return await _repository.Add(entityMaps);
        }
    }
}
