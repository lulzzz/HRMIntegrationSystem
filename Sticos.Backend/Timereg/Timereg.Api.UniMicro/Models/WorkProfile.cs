namespace Timereg.Api.Unimicro.Models
{
    public class WorkProfile
    {
        public int Id { get; set; }
        public bool LunchIncluded { get; set; }
        public int MinutesPerMonth { get; set; }
        public int MinutesPerWeek { get; set; }
        public int MinutesPerYear { get; set; }

    }
}