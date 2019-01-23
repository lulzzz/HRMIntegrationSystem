namespace Common.Api.Contracts.Users
{
    public interface ISearchQueryUser : Shared.Contracts.ISearchQuery
    {
        int? UnitId { get; set; }
    }

    public class SearchQueryUser : ISearchQueryUser
    {
        public int? UnitId { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
