using System.Collections.Generic;
using Shared.Domain.Enums;

namespace Common.Api.Domain.Entities
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public string LegalOrganizationNumber { get; set; }
        public string BusinessOrganizationNumber { get; set; }
        public int? ParentId { get; set; }
    }
}
