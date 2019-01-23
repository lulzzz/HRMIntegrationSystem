namespace Altinn.Api.Domain.Entities
{
    public class NavMessage
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        public string MessageXml { get; set; }
        public string Namespace { get; set; }
        public IntegrationType IntegrationType { get; set; }
        public WorkState WorkState { get; set; }
        public string ReferenceId { get; set; }
        public string BusinessOrganizationNumber { get; set; }
        public string ReporteeId { get; set; }
        public string MesssageId { get; set; }
        public string AttachmentId { get; set; }
    }
}








