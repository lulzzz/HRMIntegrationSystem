using Altinn.Api.Domain.Entities;

namespace Altinn.Api.Contratcs
{
    public class SearchQueryNavMessage: SearchQuery
    {
        public string Namespace { get; set; }
        public IntegrationType? IntegrationType { get; set; }
        public WorkState? WorkState { get; set; }
        public string BusinessOrganizationNumber { get; set; }
    }
}
