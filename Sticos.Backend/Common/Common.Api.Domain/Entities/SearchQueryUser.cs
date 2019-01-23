using Common.Api.Domain.Interfaces.Users;

namespace Common.Api.Domain.Entities
{
    public class SearchQueryUser : ISearchQueryUser
    {
        public int? UnitId { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}