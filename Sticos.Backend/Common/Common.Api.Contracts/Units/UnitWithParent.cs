using System.Collections.Generic;

namespace Common.Api.Contracts
{
    public class UnitWithParent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public int Type { get; set; }
    }
}