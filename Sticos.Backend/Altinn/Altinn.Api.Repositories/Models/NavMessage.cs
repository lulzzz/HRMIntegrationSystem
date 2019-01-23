using Altinn.Api.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Altinn.Api.Repositories.Models
{
    public class NavMessage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string ExternalId { get; set; }

        [Required]
        [MaxLength]
        public string MessageXml { get; set; }

        [Required]
        [StringLength(255)]
        public string Namespace { get; set; }

        [Required]
        public IntegrationType IntegrationType { get; set; }

        [Required]
        public WorkState WorkState { get; set; }

        [StringLength(255)]
        public string ReferenceId { get; set; }

        [Required]
        [StringLength(255)]
        public string BusinessOrganizationNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string ReporteeId { get; set; }

        [Required]
        [StringLength(255)]
        public string MesssageId { get; set; }

        [Required]
        [StringLength(255)]
        public string AttachmentId { get; set; }
    }
}
