namespace Shared.Contracts
{
    public class EntityMatch
    {
        public string EntityMap { get; set; }
        public int EntityId { get; set; }
        public double MatchFactor { get; set; }
        public ExternalData ExternalData { get; set; }
    }
}
