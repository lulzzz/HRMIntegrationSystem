using Common.Api.Contracts;

namespace Common.Api.Domain.Entities
{
    public class SearchQueryNotification : Shared.Contracts.SearchQueryBase
    {
        public string Title { get; set; }
    }
}