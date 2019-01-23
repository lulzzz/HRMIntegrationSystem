namespace Integrations.Api.Repositories.Models
{
    public class Integration
    {
        public int Id { get; set; }

        public bool IsActivated { get; set; }

        public int UnitId { get; set; }

        public int Category { get; set; }

        public int ExternalSystem { get; set; }
    }
}
