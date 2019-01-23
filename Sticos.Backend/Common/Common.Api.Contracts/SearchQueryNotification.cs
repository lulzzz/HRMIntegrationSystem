using Shared.Contracts;

namespace Common.Api.Contracts
{
    public class SearchQueryNotification : SearchQueryBase
    {
        public string Title { get; set; }
    }
}