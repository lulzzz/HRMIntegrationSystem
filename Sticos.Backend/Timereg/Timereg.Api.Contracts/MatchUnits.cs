using Common.Api.Contracts;

namespace Timereg.Api.Contracts
{
    public class MatchUnits
    {
        public Unit LocalUnit { get; set; }
        public Unit ExternalUnit { get; set; }

        public bool IsConfirmed { get; set; }
        public bool IsIgnored { get; set; }
        public float ProcentMatch { get; set; }
        public string MapPropertyKey { get; set; }
    }
}
