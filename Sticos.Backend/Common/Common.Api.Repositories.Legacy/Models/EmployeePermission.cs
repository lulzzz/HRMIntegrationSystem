using System.ComponentModel.DataAnnotations.Schema;
using Common.Api.Domain.Interfaces.Employees;

namespace Common.Api.Repositories.Legacy.Models
{
    [Table("AnsattRettighet")]
    public class EmployeePermission
    {
        public int Id { get; set; }
        
        [Column("AnsvarligUserId")]
        public int ResponsibleUserId { get; set; }

        [Column("AnsvarligForUserId")]
        public int ResponsibleForUserId { get; set; }

        [Column("RettighetId")]
        public int PermissionType { get; set; }

        [Column("ErEksplisitt")]
        public bool IsExplicit { get; set; }

    }
}