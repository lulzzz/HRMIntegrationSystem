using System;
using System.Collections.Generic;
using System.Text;

namespace Altinn.Api.Domain.Entities
{
    public class Reportee
    {
        public string ReporteeId { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string OrganizationNumber { get; set; }
    }
}
