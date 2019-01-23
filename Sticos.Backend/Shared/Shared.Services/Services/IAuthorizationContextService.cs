namespace Shared.Services.Services
{
    public interface IAuthorizationContextService
    {
        bool IsUserContext();
        string GetSecretFromHeader();
        int GetUserIdFromClaims();
        int GetCustomerIdFromClaims();
    }
}