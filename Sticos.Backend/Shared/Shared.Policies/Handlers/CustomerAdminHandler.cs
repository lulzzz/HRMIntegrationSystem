using Common.Api.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Shared.Policies.Requirements;
using Shared.Services.Services;
using System.Threading.Tasks;

namespace Shared.Policies
{
    public class CustomerAdminHandler : AuthorizationHandler<CustomerAdminRequirement>
    {
        public IAuthorizationContextService _authContext;
        private IUserService _userService;

        public CustomerAdminHandler(IAuthorizationContextService authContext, IUserService userService)
        {
            _authContext = authContext;
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomerAdminRequirement requirement)
        {
            if (!_authContext.IsUserContext())
            {
                return;
            }

            var currentUser = await _userService.GetUser(_authContext.GetUserIdFromClaims());

            if (currentUser.IsPersonalCustomerAdmin)
            {
                context.Succeed(requirement);
            }

            return;
        }
    }
}
