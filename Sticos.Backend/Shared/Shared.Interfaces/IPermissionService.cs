using Shared.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionType>> GetByUnitType(UnitType unitType);
        Task<IEnumerable<PermissionType>> GetImplicitByUnitType(UnitType unitType);
    }
}
