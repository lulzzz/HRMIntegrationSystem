using Microsoft.Extensions.Logging;
using Shared.Domain.Enums;
using Shared.Domain.ValueObjects.Queries;
using Shared.Interfaces;
using Shared.Interfaces.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Services.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAuthorizationQueries _authorizationQueries;
        private readonly IPermissionService _permissionService;
        private readonly IUnitQueries _unitQueries;
        private readonly ILogger<AuthorizationService> _logService;

        public AuthorizationService(
            IAuthorizationQueries authorizationQueries,
            IPermissionService permissionService,
            IUnitQueries unitQueries,
            ILogger<AuthorizationService> logService)
        {
            _authorizationQueries = authorizationQueries ?? throw new ArgumentNullException(nameof(authorizationQueries));
            _permissionService = permissionService ?? throw new ArgumentNullException(nameof(permissionService));
            _unitQueries = unitQueries ?? throw new ArgumentNullException(nameof(unitQueries));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<IEnumerable<UnitPermission>> GetUnitPermissions(int userId, int? unitId = null, params PermissionType[] permissionTypes)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));
            if (unitId.HasValue && (unitId.Value == 0 || unitId.Value < UnitConstants.MasterUnitId)) throw new ArgumentOutOfRangeException(nameof(unitId));

            var list = new List<UnitPermission>();

            if (permissionTypes == null)
            {
                permissionTypes = new PermissionType[] { };
            }

            // Add all implicit permissions for master unit
            var implicitMasterPermissions = await _permissionService.GetImplicitByUnitType(UnitType.Master);
            var masterPermissions = implicitMasterPermissions.Intersect(permissionTypes);
            list.AddRange(masterPermissions.Select(x => new UnitPermission { UnitId = UnitConstants.MasterUnitId, PermissionType = x }));

            // Get raw list of units and permissions
            var explicitUnitPermissions = _authorizationQueries.GetUnitPermissionsByUserId(userId, unitId, permissionTypes).Distinct();
            if (!explicitUnitPermissions.Any())
            {
                return list;
            }

            list.AddRange(explicitUnitPermissions);

            // Figure out which permissions the user has on departments and which of these permissions should be for the company
            var unitIdList = list.Select(x => x.UnitId).Distinct();
            var units = await _unitQueries.GetByIdList(unitIdList);
            var departments = units.Where(x => x.Type == UnitType.Department);

            if (!departments.Any())
            {
                return list;
            }

            var departmentPermissions = await _permissionService.GetByUnitType(UnitType.Department);

            foreach (var department in departments)
            {
                // Find all permissions that should be on company
                var currentPermissions = list.Where(x => x.UnitId == department.Id).Select(x => x.PermissionType);
                var permissionsToMove = currentPermissions.Except(departmentPermissions);
                if (!permissionsToMove.Any())
                {
                    continue;
                }

                // Find the parent company
                var parents = await _unitQueries.GetHierarchyUp(department.Id);
                var companyParent = parents.SingleOrDefault(x => x.Type == UnitType.Company);
                if (companyParent == null)
                {
                    _logService.LogWarning($"Department {department.Id} has no valid company parent");
                    continue;
                }

                _logService.LogTrace($"Permissions {string.Join(", ", permissionsToMove.Select(x => x.ToString()))} for userId {userId} replaced for unitId {department.Id} to {companyParent.Id}");

                // Move all the non-department permissions to company
                list.ForEach(x =>
                {
                    if (x.UnitId == department.Id && permissionsToMove.Contains(x.PermissionType))
                    {
                        x.UnitId = companyParent.Id;
                    }
                });
            }

            return list;
        }

        public async Task<IEnumerable<UserPermission>> GetUserPermissions(int userId, int? responsibleForUserId = null, params PermissionType[] permissionTypes)
        {
            return await Task.FromResult(Enumerable.Empty<UserPermission>());
        }

        public async Task<bool> HasAllPermissions(int userId, int unitId, params PermissionType[] permissionTypes)
        {
            if (userId <= 0) throw new ArgumentNullException(nameof(userId));
            if (unitId == 0 || unitId < UnitConstants.MasterUnitId) throw new ArgumentNullException(nameof(unitId));
            if (permissionTypes == null || !permissionTypes.Any()) throw new ArgumentNullException(nameof(permissionTypes));

            var unitPermissions = await GetUnitPermissions(userId, unitId, permissionTypes);

            return unitPermissions.Where(x => x.UnitId == unitId).All(x => permissionTypes.Contains(x.PermissionType));
        }

        public async Task<bool> HasAnyPermission(int userId, int unitId, params PermissionType[] permissionTypes)
        {
            if (userId <= 0) throw new ArgumentNullException(nameof(userId));
            if (unitId == 0 || unitId < UnitConstants.MasterUnitId) throw new ArgumentNullException(nameof(unitId));
            if (permissionTypes == null || !permissionTypes.Any()) throw new ArgumentNullException(nameof(permissionTypes));

            var unitPermissions = await GetUnitPermissions(userId, unitId, permissionTypes);

            return unitPermissions.Where(x => x.UnitId == unitId).Any(x => permissionTypes.Contains(x.PermissionType));
        }

        public async Task<bool> IsCustomerAdmin(int userId)
        {
            var userPermissions = await GetUserPermissions(userId, userId, PermissionType.AdminKunde);

            return userPermissions.Any(x => x.UserId == x.ResponsibleForUserId && x.UserId == userId && x.PermissionType == PermissionType.AdminKunde);
        }
    }
}
