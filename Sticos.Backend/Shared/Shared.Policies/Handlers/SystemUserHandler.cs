using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Shared.Services.Constants;
using Shared.Services.Extensions;
using Shared.Services.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Policies.Handlers
{
    public class SystemUserHandler : IAuthorizationHandler
    {
        private readonly IAuthorizationContextService _authContext;
        private readonly string _secret;
        private readonly string _secretKey = AuthConstants.SystemUserSecret;

        public SystemUserHandler(IAuthorizationContextService authContext, IConfiguration config)
        {
            _authContext = authContext;
            _secret = config.GetValueNotNull<string>(_secretKey);
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            // If we are a user we return so the other Handlers can handle it
            if (_authContext.IsUserContext())
            {
                return Task.CompletedTask;
            }

            var pendingRequirements = context.PendingRequirements.ToList();
            var headerSecret = _authContext.GetSecretFromHeader();

            foreach (var req in pendingRequirements)
            {
                if (headerSecret == _secret)
                {
                    context.Succeed(req);
                }
            }

            return Task.CompletedTask;
        }
    }
}
