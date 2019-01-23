namespace Timereg.Api.Contracts
{
    public class MatchAbsenceType
    {
        public int LocalAbsenceType { get; set; }
        public string ExternalAbsenceType { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsIgnored { get; set; }
        public float ProcentMatch { get; set; }
    }
}
