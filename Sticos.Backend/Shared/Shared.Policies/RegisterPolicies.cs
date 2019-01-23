using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shared.Policies.Handlers;
using Shared.Policies.Requirements;
using Shared.Services.Constants;

namespace Shared.Policies
{
    public static class RegisterPolicies
    {
        private static AuthorizeFilter CustomerIdFilter()
        {
            var globalFilter = new AuthorizationPolicyBuilder()
                .AddRequirements(new CustomerIdRequirement())
                .Build();

            return new AuthorizeFilter(globalFilter);
        }

        private static AuthorizeFilter CustomerAdminFilter()
        {
            var globalFilter = new AuthorizationPolicyBuilder()
                    .AddRequirements(new CustomerAdminRequirement())
                    .Build();

            return new AuthorizeFilter(globalFilter);
        }

        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                o.Filters.Add(CustomerIdFilter());
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthConstants.CustomerAdminPolicyName, policy =>
                {
                    policy.Requirements.Add(new CustomerAdminRequirement());
                });
            });

            services.AddSingleton<IAuthorizationHandler, CustomerIdHandler>();
            services.AddSingleton<IAuthorizationHandler, SystemUserHandler>();
            services.AddSingleton<IAuthorizationHandler, CustomerAdminHandler>();
        }

        public static void AddPoliciesGlobally(this IServiceCollection services)
        {
            services.AddMvc(o =>
            {
                o.Filters.Add(CustomerAdminFilter());
                o.Filters.Add(CustomerIdFilter());
            });

            services.AddSingleton<IAuthorizationHandler, CustomerIdHandler>();
            services.AddSingleton<IAuthorizationHandler, SystemUserHandler>();
            services.AddSingleton<IAuthorizationHandler, CustomerAdminHandler>();
        }
    }
}
