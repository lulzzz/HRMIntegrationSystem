using Sticos.Personal.MessageContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Integrations.Api.Domain.Models
{
    public class EmployeeDeleted : IEmployeeDeleted
    {
        public int EmployeeId { get; set; }
        public int OrgUnitId { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
    }
}
