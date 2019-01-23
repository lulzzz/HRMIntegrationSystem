using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Api.Repositories.Legacy.Models
{  
    [Table("OrgUnitVerification")]
    public class OrgUnitVerification
    {
        public int Id { get; set; }

        [ForeignKey("OrgUnitId")]
        public Unit Unit { get; set; }

        public int OrgUnitId { get; set; }
        public OrgNrStatus Status { get; set; }
        public string MainId { get; set; }
        public string SubId { get; set; }
        
    }

    public enum OrgNrStatus
    {
        Unknown = 0,
        Pending =1,
        Verified = 2,
    }
}