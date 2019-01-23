namespace Integrations.Api.Contracts
{
    public class SearchQueryIntegration : SearchQueryBase
    {
        public int UnitId { get; set; }
        public int Category { get; set; }
        public int ExternalSystemId { get; set; }

    }
}
