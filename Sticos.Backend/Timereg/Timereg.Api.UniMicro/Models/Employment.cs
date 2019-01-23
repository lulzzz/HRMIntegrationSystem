using System;

namespace Timereg.Api.Unimicro.Models
{
    public class Employment
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string JobCode { get; set; }
        public string JobName { get; set; }
        public double WorkPercent { get; set; }
        public double HoursPerWeek { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool Deleted { get; set; }
    }
}