using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Api.Repositories.Models
{
    [Table("Dashboards")]
    public class Dashboard
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string DashboardConfig { get; set; }

        public int? OwnerTypeId { get; set; }
        public OwnerType OwnerType { get; set; }
        public int? OwnerId { get; set; }
        public bool IsDefault { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
    }
}