using Shared.Interfaces;
using Shared.Interfaces.Models;
using Shared.Services.Models;

namespace Shared.Services.Services
{
    public class TokenUserContext : ICurrentUserContext
    {
        private readonly IAuthorizationContextService _authContext;

        public TokenUserContext(IAuthorizationContextService authContext)
        {
            _authContext = authContext;
        }
        public IUserContext Get()
        {
            var userId = _authContext.GetUserIdFromClaims();
            return new UserContext { UserId = userId };
        }
    }
}
