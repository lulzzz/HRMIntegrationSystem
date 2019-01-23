using Shared.Domain.Enums;
using Shared.Domain.ValueObjects.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> IsCustomerAdmin(int userId);
        Task<bool> HasAnyPermission(int userId, int unitId, params PermissionType[] permissionTypes); // One or more
        Task<bool> HasAllPermissions(int userId, int unitId, params PermissionType[] permissionTypes); // All

        Task<IEnumerable<UnitPermission>> GetUnitPermissions(int userId, int? unitId = null, params PermissionType[] permissionTypes);
        Task<IEnumerable<UserPermission>> GetUserPermissions(int userId, int? responsibleForUserId = null, params PermissionType[] permissionTypes);
    }
}
