
namespace Timereg.Api.Contracts
{
    public class SearchQueryAbsenceExport : Shared.Contracts.SearchQueryBase
    {
        public int LocalId { get; set; }
        public int UnitId { get; set; }
        public string Id { get; set; }
    }
}
