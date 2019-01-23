using Microsoft.AspNetCore.Http;
using Shared.Services.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using sticos = Sticos.Foundation.Login.IdentityManager.Common;

namespace Shared.Services.Services
{
    public class AuthorizationContextService : IAuthorizationContextService
    {
        private readonly IHttpContextAccessor _httpContext;

        public AuthorizationContextService(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }

        public bool IsUserContext()
        {
            var userId = GetUserIdFromClaims();
            var customerId = GetCustomerIdFromClaims();
            // If we are a user we return so the other Handlers can handle it
            if (userId > 0 && customerId > 0)
            {
                return true;
            }
            return false;
        }

        public string GetSecretFromHeader()
        {
            _httpContext.HttpContext?.Request?.Headers?.TryGetValue(AuthConstants.SystemUserSecret, out var headerSecret);
            return headerSecret;
        }

        public int GetUserIdFromClaims()
        {
            var claims = GetClaims();

            var uIdString = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            int.TryParse(uIdString, out int userId);

            return userId;
        }

        private List<Claim> GetClaims()
        {
            return ((ClaimsIdentity)_httpContext?.HttpContext?.User?.Identity)?.Claims?.ToList();
        }

        public int GetCustomerIdFromClaims()
        {
            var claims = GetClaims();
            var cIdString = claims?.FirstOrDefault(c => c.Type == sticos.Constants.ClaimTypes.Kundeid)?.Value;
            if (string.IsNullOrEmpty(cIdString)) cIdString = claims?.FirstOrDefault(c => c.Type == Sticos.Bibliotek.Claims.Fellestjenester.Claimstjenester.tokenserviceClaimTypeKundeid)?.Value;
            int.TryParse(cIdString, out int customerId);

            return customerId;
        }
    }

    public class StaticAuthorizationContextService : IAuthorizationContextService
    {
        private int _customerId;
        private int _userId;
        private string _secret;

        public StaticAuthorizationContextService(int customerId, int userId, string secret)
        {
            _customerId = customerId;
            _userId = userId;
            _secret = secret;
        }
        public int GetCustomerIdFromClaims()
        {
            return _customerId;
        }

        public string GetSecretFromHeader()
        {
            return _secret;
        }

        public int GetUserIdFromClaims()
        {
            return _userId;
        }

        public bool IsUserContext()
        {
            return true;
        }
    }
}
