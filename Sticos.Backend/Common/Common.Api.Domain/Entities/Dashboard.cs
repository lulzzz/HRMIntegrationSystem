using System;

namespace Common.Api.Domain.Entities
{
    public class Dashboard
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string DashboardConfig { get; set; }
        public int OwnerTypeId { get; set; }
        public int? OwnerId { get; set; }
        public bool IsDefault { get; set; }
        public DateTimeOffset? DateCreated { get; set; }
    }
}