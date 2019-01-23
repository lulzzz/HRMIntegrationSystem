using System;
using System.Collections.Generic;
using System.Text;

namespace Integrations.Api.Domain.Models
{
    public class SearchQueryEntityMap : SearchQueryBase
    {
        public int IntegrationId { get; set; }

        public int LocalId { get; set; }
        public string EntityName { get; set; }
        public string ExternalProperty { get; set; }
        public string ExternalEntity { get; set; }

        public int UnitId { get; set; }
        public int ExternalSystemId { get; set; }
        public int IntegrationCategory { get; set; }
    }
}
