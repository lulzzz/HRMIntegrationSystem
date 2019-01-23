using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Interfaces.Queries;
using Shared.Queries;
using Shared.Services.Services;

namespace Shared.Services.Extensions
{
    public static class IServiceCollectionSharedExtensions
    {
        public static void AddSharedServices(this IServiceCollection services)
        {
            services.AddScoped<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<ICustomerIdService, CustomerIdService>();
            services.AddScoped<IStaticCustomerId, StaticCustomerId>();

            services.AddScoped<IUnitQueries, UnitQueries>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IAuthorizationQueries, AuthorizationQueries>();

            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));
            services.AddScoped(typeof(IEntityService<,,>), typeof(EntityService<,,>));
        }
    }
}
