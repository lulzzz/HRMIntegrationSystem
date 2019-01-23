using System.Collections.Generic;

namespace Timereg.Api.Contracts
{
    public class Absence
    {
        public int LocalId { get; set; }

        public int UnitId { get; set; }
        
        public int EmployeeId { get; set; }

        public int LunchInMinutes { get; set; }

        public string Description { get; set; }

        public List<AbsenceEntry> AbsenceEntries { get; set; }
    }
}
