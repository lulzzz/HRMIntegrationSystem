using Common.Api.Contracts.Users;
using Microsoft.AspNetCore.Mvc;
using Shared.Services.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Common.Api.Controllers
{
    [Route("currentuser")]
    public class CurrentUserController : Controller
    {
        private IAuthorizationContextService _authorizationContext;

        public CurrentUserController(IAuthorizationContextService authorizationContext)
        {
            _authorizationContext = authorizationContext;
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        public ActionResult<IClaimsUser> GetCurrentUser()
        {
            return Ok(new { UserId = _authorizationContext.GetUserIdFromClaims(), CustomerId = _authorizationContext.GetCustomerIdFromClaims() });
        }
    }
}
