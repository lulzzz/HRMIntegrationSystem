namespace Common.Api.Domain.Entities
{
    public class SearchQueryAnomaly : Shared.Contracts.SearchQueryBase
    {
        public string Location { get; set; }
        public string Responsible { get; set; }
    }
}