using System.Collections.Generic;

namespace Integrations.Api.Domain.Models
{
    public class SearchQueryIntegration : SearchQueryBase
    {
        public int UnitId { get; set; }
        public List<int> UnitIds { get; set; } = new List<int>();
        public int Category { get; set; }
        public int ExternalSystemId { get; set; }

        public SearchQueryIntegration(int unitId, int category, int externalSystemId)
        {
            UnitId = unitId;
            Category = category;
            ExternalSystemId = externalSystemId;
        }
        public SearchQueryIntegration() { }
    }
}
