using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Services.Constants;
using Shared.Services.Models;
using Shared.Services.Services;
using System;

namespace Shared.Services.Extensions
{
    public static class IServiceCollectionMvcAuthorizeExtensions
    {
        public static IServiceCollection AddMvc(this IServiceCollection services, bool addAuthorization, IConfiguration configuration, Type[] globalControllers = null)
        {
            if (addAuthorization)
            {
                services.AddMvc(options =>
                   {
                       var policy = new AuthorizationPolicyBuilder()
                           .RequireAuthenticatedUser()
                           .Build();
                       options.Filters.Add(new AuthorizeFilter(policy));
                       options.UseCustomerIdRoutePrefix(globalControllers);
                   });
                services.AddJwtAuthentication(configuration);

                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                services.AddScoped<ICurrentUserContext, TokenUserContext>();
                services.AddTransient<IAuthorizationContextService, AuthorizationContextService>();
            }
            else
            {
                services.AddMvc(options =>
                {
                    options.UseCustomerIdRoutePrefix(globalControllers);
                    options.Filters.Add(new AllowAnonymousFilter());
                });

                var userContext = new StaticUserContext(new UserContext { UserId = 89727 });
                services.AddSingleton<ICurrentUserContext>(userContext);
                var authContextService = new StaticAuthorizationContextService(400423, 89727, configuration.GetValue<string>(AuthConstants.SystemUserSecret));
                services.AddSingleton<IAuthorizationContextService>(authContextService);
            }
            return services;
        }
    }
}
