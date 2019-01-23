using Shared.Domain.Enums;

namespace Shared.Domain.ValueObjects.Queries
{
    public class UserPermission
    {
        public int UserId { get; set; }
        public int ResponsibleForUserId { get; set; }
        public PermissionType PermissionType { get; set; }
        public bool IsExplicit { get; set; }
    }
}
