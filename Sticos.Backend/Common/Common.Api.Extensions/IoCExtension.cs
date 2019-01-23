using Common.Api.Domain.Entities;
using Common.Api.Domain.HttpClientHelper;
using Common.Api.Domain.Interfaces;
using Common.Api.Domain.Interfaces.Employees;
using Common.Api.Domain.Interfaces.Repositories;
using Common.Api.Domain.Interfaces.Users;
using Common.Api.Domain.Services;
using Common.Api.Domain.Validators;
using Common.Api.Domain.Validators.Interfaces;
using Common.Api.Repositories.Context;
using Common.Api.Repositories.ContextFactory;
using Common.Api.Repositories.Legacy.Context;
using Common.Api.Repositories.Legacy.Factories;
using Common.Api.Repositories.Legacy.Repositories;
using Common.Api.Repositories.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;
using Shared.Services.Extensions;
using Shared.Services.Services;
using System.Collections.Generic;

namespace Common.Api.Extensions
{
    public static class IoCExtension
    {
        public static void AddIocMapping(this IServiceCollection services)
        {
            services.AddScoped<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IOwnerTypeService, OwnerTypeService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<Contracts.Users.IUserService, Domain.Services.Proxy.UserProxyService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IAnomalyService, WidgetMockDataService>();
            services.AddScoped<INotificationService, WidgetMockDataService>();
            services.AddScoped<IUnitService, UnitService>();

            services.AddScoped<IBrregService, BrregService>();

            services.AddScoped<IRepository<Dashboard, SearchQueryDashboard>, DashboardRepository>();
            services.AddScoped<IRepository<OwnerType, SearchQueryOwnerType>, OwnerTypeRepository>();
            services.AddScoped<IBrregRepository, BrregRepository>();
            services.AddScoped<IEntityValidator<Dashboard, int?>, DashboardValidator>();

            services.AddTransient<IDbContextFactory<SticosWidgetDbContext>, DbContextFactory>();

            // Legacy
            services.AddScoped<IDbContextFactory<PersonalLegacyContext>, PersonalLegacyContextFactory>();
            services.AddScoped<IDbContextFactory<PersonalCommonLegacyContext>, PersonalCommonLegacyContextFactory>();
            services.AddScoped<IUnitRepository, UnitLegacyDbEFRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeLegacyDbEfRepository>();
            services.AddScoped<IAbsenceTypeService, AbsenceTypeService>();
            services.AddScoped<IUserRepository, UserLegacyDbEfRepository>();

        }

        public static void AddJsonHttpClients(this IServiceCollection services)
        {
            services.AddJsonHttpClients(new Dictionary<string, string>
            {
                {
                    HttpClientConfiguration.BrregClient, HttpClientConfiguration.BrregUrl
                }
            });
        }
    }
}
