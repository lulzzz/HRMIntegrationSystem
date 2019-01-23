using System.Linq;
using System.Threading.Tasks;
using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;
using Timereg.Api.Domain.Models;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Unimicro.Adapters.UniMicro
{
    public static class EntityMapServiceExtensions
    {
        public static async Task<int?> GetExternalId(this IEntityMapService entityMapService, 
            EntityType entityType, 
            IdentifierEntity idEntity, 
            IdentifierProperty property, 
            int localId,
            int unitId)
        {
            var entityMaps = await entityMapService.SearchEntityMaps(
                new SearchQueryEntityMap(
                    localId, 
                    entityType.ToString(),
                    unitId,
                    (int)ExternalEconomySystem.UniMicro,
                    (int)Category.Timereg)
            {
                ExternalEntity = idEntity.ToString(),
                ExternalProperty = property.ToString()
            });
            var value = entityMaps.FirstOrDefault()?.ExternalValue;
            return int.TryParse(value, out int intValue) && intValue > 0 
                ? (int?) intValue 
                : null;
        }
        public static async Task<int?> GetWorkRelationId(this IEntityMapService entityMapService, int localEmployeeId, int unitId)
        {
            return await entityMapService.GetExternalId(
                EntityType.Employee,
                IdentifierEntity.WorkRelation,
                IdentifierProperty.Id,
                localEmployeeId,
                unitId);
        }
       
        public static async Task<int?> GetWorkTypeId(this IEntityMapService entityMapService, int localAbsenceCode, int unitId)
        {
            return await entityMapService.GetExternalId(
                EntityType.AbsenceType,
                IdentifierEntity.WorkType,
                IdentifierProperty.Id,
                localAbsenceCode,
                unitId);
        }

        public static async Task<int?> GetWorkerId(this IEntityMapService entityMapService, int localEmployeeId, int unitId)
        {
            return await entityMapService.GetExternalId(
                EntityType.Employee,
                IdentifierEntity.Worker,
                IdentifierProperty.Id,
                localEmployeeId,
                unitId);
        }

        public static async Task<int?> GetEmploymentId(this IEntityMapService entityMapService,
            int localEmployeeId, int unitId)
        {
            return await entityMapService.GetExternalId(
                EntityType.Employee,
                IdentifierEntity.Employment,
                IdentifierProperty.Id,
                localEmployeeId,
                unitId);
        }
        public static async Task<int?> GetEmploymentLeaveId(this IEntityMapService entityMapService,
            int localAbsenceCode, int unitId)
        {
            return await entityMapService.GetExternalId(
                EntityType.AbsenceType,
                IdentifierEntity.EmploymentLeaveType,
                IdentifierProperty.Id,
                localAbsenceCode,
                unitId);
        }
    }
}