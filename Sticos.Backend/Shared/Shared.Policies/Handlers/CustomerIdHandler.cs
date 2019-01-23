using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Interfaces;
using Shared.Policies.Requirements;
using Shared.Services.Services;
using System.Threading.Tasks;

namespace Shared.Policies.Handlers
{
    public class CustomerIdHandler : AuthorizationHandler<CustomerIdRequirement>
    {
        private readonly ICustomerIdService _customerIdService;

        public IAuthorizationContextService _authContext;

        public CustomerIdHandler(IAuthorizationContextService authContext, ICustomerIdService customerIdService)
        {
            _customerIdService = customerIdService;
            _authContext = authContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomerIdRequirement requirement)
        {
            if (!_authContext.IsUserContext())
            {
                return Task.CompletedTask;
            }

            var claimsCustomerId = _authContext.GetCustomerIdFromClaims();

            var asd = context.Resource as AuthorizationFilterContext;

            var customerId = _customerIdService.GetCustomerId();

            if (!customerId.HasValue || customerId.Value == claimsCustomerId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
