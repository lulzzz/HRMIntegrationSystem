namespace Common.Api.Contracts.Users
{
    public interface IUser
    {
        int? UserId { get; }
        int? UnitId { get; }
        int? CustomerId { get; }
        string Email { get; }
        string Mobilephone { get; }
        string Workphone { get; }
        string FirstName { get; }
        string LastName { get; }
        bool IsPersonalCustomerAdmin { get; }
    }
}
