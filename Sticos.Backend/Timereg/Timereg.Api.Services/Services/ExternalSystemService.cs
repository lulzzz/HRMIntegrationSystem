using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Timereg.Api.Domain.Interfaces;
using Timereg.Api.Domain.Models;
using Shared.Contracts;

namespace Timereg.Api.Services.Services
{
    public class ExternalSystemService : IExternalSystemService
    {
        private readonly IExternalSystemRepository _repository;
        private readonly IExternalSystemFactory _externalSystemFactory;
        public ExternalSystemService(
            IExternalSystemRepository repository,
            IExternalSystemFactory externalSystemFactory
            )
        {
            _repository = repository;
            _externalSystemFactory = externalSystemFactory;
        }

        public async Task<IEnumerable<ExternalSystem>> Search(SearchQueryExternalSystem query)
        {
            var externalSystems = await _repository.Search();
            return externalSystems.OrderBy(e => e.Order);
        }

        public async Task<IEnumerable<ExternalData>> GetExternalData(int externalSystem, int unitId, int entityType)
        {
            var externalAdapter = _externalSystemFactory.CreateDataService((ExternalEconomySystem)externalSystem);

            switch ((ExternalDataEnum)entityType)
            {
                case ExternalDataEnum.Employee:
                    return await externalAdapter.GetExternalEmployeeData(unitId);
                case ExternalDataEnum.Unit:
                    return await externalAdapter.GetExternalUnitData(unitId);
                case ExternalDataEnum.AbsenceCode:
                    return await externalAdapter.GetExternalAbsenceCodeData(unitId);
                default:
                    return null;
            }
        }

        public async Task<IEnumerable<EntityMatch>> MatchEntities(int externalSystem, int unitId, int entityType, int[] ids)
        {
            var externalAdapter = _externalSystemFactory.CreateMatchingService((ExternalEconomySystem)externalSystem);

            switch ((ExternalDataEnum)entityType)
            {
                case ExternalDataEnum.Employee:
                    return await externalAdapter.MatchEmployeeData(unitId, ids);
                case ExternalDataEnum.Unit:
                    return await externalAdapter.MatchUnitData(unitId, ids);
                case ExternalDataEnum.AbsenceCode:
                    return await externalAdapter.MatchAbsenceCodeData(unitId, ids);
                default:
                    return null;
            }
        }

        public async Task<ExternalSystem> GetExternalSystem(int externalSystem)
        {
            return await _repository.GetExternalSystem(externalSystem);
        }
    }
}
