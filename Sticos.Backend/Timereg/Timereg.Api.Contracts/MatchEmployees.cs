using Common.Api.Contracts.Employees;

namespace Timereg.Api.Contracts
{
    public class MatchEmployees
    {
     public IEmployee LocalEmployee { get; set; }
     public ExternalEmployeeData ExternalEmployee { get; set; }
     
     public bool IsConfirmed { get; set; }
     public bool IsIgnored { get; set; }
     public float ProcentMatch { get; set; }
     public string MapPropertyKey { get; set; }
    }
}
