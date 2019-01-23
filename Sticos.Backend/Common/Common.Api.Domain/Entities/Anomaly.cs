using System;

namespace Common.Api.Domain.Entities
{
    public class Anomaly
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Responsible { get; set; }
        public DateTimeOffset Deadline { get; set; }
        public string Status { get; set; }
    }
}