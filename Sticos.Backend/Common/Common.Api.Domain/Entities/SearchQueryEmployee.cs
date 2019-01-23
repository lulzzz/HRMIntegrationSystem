using Common.Api.Domain.Interfaces.Employees;
using System.Collections.Generic;

namespace Common.Api.Domain.Entities
{
    public class SearchQueryEmployee : ISearchQueryEmployee
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string SocialSecurityNumber { get; set; }
        public List<int> EmployeesIds { get; set; }
        public List<int> UserIds { get; set; }
        public List<int> UnitIds { get; set; }
    }
}
