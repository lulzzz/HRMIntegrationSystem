namespace Timereg.Api.Contracts
{
    public class MatchAbsenceExport
    {
       public Absence Absence { get; set; }

        public AbsenceExportStatus Status { get; set; }

        public string Message { get; set; }
    }
}
