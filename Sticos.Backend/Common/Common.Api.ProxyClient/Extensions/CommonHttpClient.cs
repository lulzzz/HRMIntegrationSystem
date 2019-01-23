using System;
using System.Net.Http.Headers;
using Common.Api.Contracts.Employees;
using Common.Api.Contracts.Services;
using Common.Api.Contracts.Users;
using Common.Api.ProxyClient.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Services.Extensions;

namespace Common.Api.ProxyClient.Extensions
{
    public static class CommonHttpClient
    {
        public static void AddCommonHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration.GetValueNotNull<string>(Constants.API_URL_CONFIG_KEY);
            services.AddHttpClient(Constants.API_CLIENT_NAME, client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            			
            services.AddScoped<IEmployeeService, CommonProxyClient>();
            services.AddScoped<IUserService, CommonProxyClient>();
            services.AddScoped<IUnitService, CommonProxyClient>();
            services.AddScoped<IAbsenceTypeService, CommonProxyClient>();
        }
    }
}
