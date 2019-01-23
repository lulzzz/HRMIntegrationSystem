using System;

namespace Timereg.Api.Unimicro.Models
{
    public class EmploymentLeave
    {
        public int Id { get; set; }
        public int EmploymentID { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public LeaveType LeaveType { get; set; }
        public decimal LeavePercent { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
    }
}