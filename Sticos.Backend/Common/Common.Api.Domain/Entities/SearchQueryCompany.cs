namespace Common.Api.Domain.Entities
{
    public class SearchQueryCompany : Shared.Contracts.SearchQueryBase
    {
        public string Location { get; set; }
        public string Responsible { get; set; }
    }
}