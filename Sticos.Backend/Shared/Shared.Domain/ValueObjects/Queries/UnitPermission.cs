using Shared.Domain.Enums;

namespace Shared.Domain.ValueObjects.Queries
{
    public class UnitPermission
    {
        public int UnitId { get; set; }
        public PermissionType PermissionType { get; set; }
    }
}
