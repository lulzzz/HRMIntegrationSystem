namespace Common.Api.Contracts.Users
{
    public interface IClaimsUser
    {
        int UserId { get; }
        int CustomerId { get; }
    }
}
