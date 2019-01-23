namespace Timereg.Api.Unimicro.Models
{
    public enum IdentifierProperty
    {
        Unknown = 0,
        Id = 1,
        SocialSecurityNumber = 2,
        OrganizationNumber = 3,
    }

    public enum IdentifierEntity
    {
        Unknown = 0,
        User = 1,
        Worker = 2,
        WorkRelation = 3,
        Employee = 4,
        Employment = 5,
        Company = 6,
        WorkType = 7,
        EmploymentLeaveType = 8,
        SubEntity = 9,
    }

    public enum EmployeeError
    {
        WorkPercentageError = 1,
        HoursOfWeekError = 2,
        WorkrelationMissingError = 3,
        EmploymentMissingError = 4,
        WorkerMissingError = 5,
    }
}
