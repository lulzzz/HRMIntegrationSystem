using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Common.Api.Repositories.Models
{
    public class OwnerType
    {
        public int Id { get; set; }

        [Required] [StringLength(255)] public string Name { get; set; }

        [Required] public int Priority { get; set; }

        public ICollection<Dashboard> Dashboards { get; } = new Collection<Dashboard>();
    }
}