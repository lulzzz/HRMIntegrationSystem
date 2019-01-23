using Shared.Domain.Enums;
using Shared.Domain.ValueObjects.Queries;
using System.Collections.Generic;

namespace Shared.Interfaces.Queries
{
    public interface IAuthorizationQueries
    {
        IEnumerable<UnitPermission> GetUnitPermissionsByUserId(int userId, int? unitId = null, params PermissionType[] permissionTypes);
    }
}
