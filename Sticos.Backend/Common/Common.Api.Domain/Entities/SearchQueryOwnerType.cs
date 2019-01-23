namespace Common.Api.Domain.Entities
{
    public class SearchQueryOwnerType : Shared.Contracts.SearchQueryBase
    {
        public string Name { get; set; }
        public int? MinPriority { get; set; }
        public int? MaxPriority { get; set; }
    }
}