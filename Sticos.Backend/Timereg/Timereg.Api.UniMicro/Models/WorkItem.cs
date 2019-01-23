using System;

namespace Timereg.Api.Unimicro.Models
{
    public class WorkItem
    {
        public int Id { get; set; }

        public DateTime Date => StartTime.Date;

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public int LunchInMinutes { get; set; }

        public int WorkRelationId { get; set; }

        public int WorkTypeId { get; set; }

        public string Description { get; set; }

        public int UnitId { get; set; }
    }
}
