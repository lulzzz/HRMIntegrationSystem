using System;

namespace Timereg.Api.Contracts
{
    public class AbsenceExport
    {
        public string Id { get;  set; }

        public int UnitId { get;  set; }

        public int LocalAbsenceId { get;  set; }

        public int EmployeeId { get;  set; }

        public AbsenceExportStatus Status { get; set; }

        public AbsenceExportAction Action { get; set; }

        public string Message { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdateAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }


        public string AbsenceJson { get;  set; }
    }
}
