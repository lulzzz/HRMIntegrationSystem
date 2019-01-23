using System.Collections.Generic;

namespace Common.Api.Contracts.Employees
{
    public interface ISearchQueryEmployee : Shared.Contracts.ISearchQuery
    {
        string FirstName { get; }
        string LastName { get; }
        string JobTitle { get; }
        string SocialSecurityNumber { get; }
        List<int> EmployeesIds { get; }
        List<int> UserIds { get; }
        List<int> UnitIds { get; }
    }
    public class SearchQueryEmployee : ISearchQueryEmployee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string SocialSecurityNumber { get; set; }
        public List<int> EmployeesIds { get; set; }
        public List<int> UserIds { get; set; }
        public List<int> UnitIds { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }
}
