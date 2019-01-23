using System;

namespace Timereg.Api.Domain.Models
{
    public class AbsenceEntry
    {
        public string ExternalId { get; set; }
        public int LocalAbsenceCode { get; set; }
        public string ExternalAbsenceCode { get; set; }
        public string ExternalEntityId { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public bool IsFullDay { get; set; }
    }
}
