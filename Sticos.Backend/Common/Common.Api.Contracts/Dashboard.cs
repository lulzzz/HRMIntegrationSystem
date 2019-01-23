using System.ComponentModel.DataAnnotations;

namespace Common.Api.Contracts
{
    public class Dashboard
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string DashboardConfig { get; set; }

        public int? OwnerTypeId { get; set; }
        public int? OwnerId { get; set; }
        public bool IsDefault { get; private set; }
    }
}