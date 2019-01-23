using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Timereg.Api.Domain.Models
{
    public class AbsenceExport
    {
        public AbsenceExport()
        {
            
        }
        public AbsenceExport(Absence absence)
        {
            UnitId = absence.UnitId;
            LocalAbsenceId = absence.LocalId;
            EmployeeId = absence.EmployeeId;
            Absence = absence;
            Status = AbsenceExportStatus.Initial;
            Action = AbsenceExportAction.Create;
        }
        public string Id { get;  set; }

        public int UnitId { get;  set;}

        public int LocalAbsenceId { get;  set;}

        public int EmployeeId { get; set;}

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? UpdateAt { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public AbsenceExportStatus Status { get; set; }
        public AbsenceExportAction Action { get; set; }

        public string Message { get; set; }
        public string AbsenceJson { get; protected set; }

        [NotMapped]
        public Absence Absence
        {
            get => JsonConvert.DeserializeObject<Absence>(AbsenceJson);
            set => AbsenceJson = JsonConvert.SerializeObject(value);
        }
    }
}
