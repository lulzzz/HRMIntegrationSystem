using Shared.Interfaces;

namespace Shared.Contracts
{
    public interface ISearchQuery 
    {
        int? Skip { get; }
        int? Take { get; }
    }

    public class SearchQueryBase : ISearchQuery
    {
        public int? Skip { get; set; } = SearchConstants.DEFAULT_SKIP;
        public int? Take { get; set; } = SearchConstants.DEFAULT_TAKE;
    }
}