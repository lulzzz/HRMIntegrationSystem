using System.ComponentModel.DataAnnotations;

namespace Common.Api.Contracts
{
    public class OwnerType
    {
        public int Id { get; set; }

        [Required] [StringLength(255)] public string Name { get; set; }

        [Required] public int Priority { get; set; }
    }
}