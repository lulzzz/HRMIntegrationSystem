using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Api.Contracts
{
    public class Anomaly
    {
        //AnomalyWidget
        public int Id { get; set; }

        [Required] public DateTimeOffset Date { get; set; }

        [Required] [StringLength(255)] public string Location { get; set; }

        [Required] [StringLength(255)] public string Description { get; set; }

        [Required] [StringLength(255)] public string Responsible { get; set; }

        [Required] public DateTimeOffset Deadline { get; set; }

        [Required] [StringLength(255)] public string Status { get; set; }
    }
}