namespace Altinn.Api.Domain.Entities
{
    public class SearchQueryNavMessage : SearchQuery
    {
        public string Namespace { get; set; }
        public IntegrationType? IntegrationType { get; set; }
        public WorkState? WorkState { get; set; }
        public string BusinessOrganizationNumber { get; set; }
    }
}