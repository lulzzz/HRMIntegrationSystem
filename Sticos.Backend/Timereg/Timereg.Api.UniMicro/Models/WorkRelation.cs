using System;

namespace Timereg.Api.Unimicro.Models
{
    public class WorkRelation
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public bool IsActive { get; set; }
        public int WorkerID { get; set; }
        public double WorkPercentage { get; set; }
        public int? WorkProfileId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public WorkProfile WorkProfile { get; set; }
    }
}