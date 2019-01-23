using System;
using System.Collections.Generic;
using System.Text;

namespace Integrations.Api.Repositories.Models
{
    public class EntityMap
    {
        public int Id { get; set; }
        public int IntegrationId { get; set; }
        public string EntityName { get; set; }
        public int EntityId { get; set; }

        public string ExternalValue { get; set; }
        public string ExternalEntity { get; set; }
        public string ExternalPropertyName { get; set; }

        public bool Ignored { get; set; }
        public bool Deleted { get; set; }
    }
}
