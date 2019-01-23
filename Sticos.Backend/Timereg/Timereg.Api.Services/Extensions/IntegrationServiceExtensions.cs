using System.Linq;
using System.Threading.Tasks;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;

namespace Timereg.Api.Services.Extensions
{
    public static class IntegrationServiceExtensions
    {
        public static async Task<Integration> GetTimeregIntegration(this IIntegrationService integrationService, int? unitId)
        {
            Integration toReturn = null;

            if (!unitId.HasValue) return toReturn;

            var searchQuery = new SearchQueryIntegration
            {
                UnitId = unitId.Value,
                Category = (int)Category.Timereg,
            };
            var integrations = await integrationService.Search(searchQuery);

            return integrations.FirstOrDefault();
        }

    }
}