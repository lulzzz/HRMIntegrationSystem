namespace Shared.Contracts
{
    public class Data
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }
    public enum PropertyName
    {
        Unknown = 0,
        Name = 1,
        Email = 2,
        UserName = 3,
        JobDescription = 5,
        OrganizationNumber = 6,
        Type = 7,
        Status = 8,
        SocialSecurityNumber = 9
    }
}
