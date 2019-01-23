using Shared.Contracts;

namespace Common.Api.Contracts
{
    public class SearchQueryOwnerType : SearchQueryBase
    {
        public string Name { get; set; }
        public int? MinPriority { get; set; }
        public int? MaxPriority { get; set; }
    }
}