using Shared.Domain.Enums;

namespace Shared.Domain.ValueObjects.Queries
{
    public class UnitWithParent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public UnitType Type { get; set; }
    }
}
