using Shared.Contracts;

namespace Common.Api.Contracts
{
    public class SearchQueryCompany : SearchQueryBase
    {
        public string Location { get; set; }
        public string Responsible { get; set; }
    }
}