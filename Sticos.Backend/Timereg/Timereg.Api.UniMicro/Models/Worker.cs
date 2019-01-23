using System.Collections.Generic;

namespace Timereg.Api.Unimicro.Models
{
    public class Worker
    {
        public int Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? BusinessReleationId { get; set; }
        public int? UserId { get; set; }
        public int CompanyId { get; set; }
        public Info Info { get; set; }
        public List<Relations> Relations { get; set; }
    }
}
