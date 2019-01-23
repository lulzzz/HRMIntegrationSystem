using System.Collections.Generic;

namespace Timereg.Api.Domain.Models
{
    public class SearchQueryAbsenceExport : Shared.Contracts.SearchQueryBase
    {
        public int LocalId { get; set; }
        public int UnitId { get; set; }
        public List<int> UnitIds { get; set; } = new List<int>();
        public string Id { get; set; }

        public SearchQueryAbsenceExport() { }
        public SearchQueryAbsenceExport(int unitId)
        {
            UnitId = unitId;
        }
    }
}
