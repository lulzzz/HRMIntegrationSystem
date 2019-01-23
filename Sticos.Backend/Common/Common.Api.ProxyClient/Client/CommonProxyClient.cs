using Common.Api.Contracts;
using Common.Api.Contracts.Employees;
using Common.Api.Contracts.Services;
using Common.Api.Contracts.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Contracts;
using Shared.Interfaces;
using Shared.Services.Constants;
using Shared.Services.Helpers;
using Shared.Services.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Common.Api.ProxyClient.Client
{
    public class CommonProxyClient : IUserService, IEmployeeService, IUnitService, IAbsenceTypeService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationContextService _authorizationContext;
        private readonly ICustomerIdService _customerIdService;

        public CommonProxyClient(IHttpClientFactory httpFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IAuthorizationContextService authorizationContext, ICustomerIdService customerIdService)
        {
            _httpFactory = httpFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _authorizationContext = authorizationContext;
            _customerIdService = customerIdService;
        }

        public async Task<IEnumerable<IEmployee>> SearchEmployee(ISearchQueryEmployee query)
        {
            return await Request<IEnumerable<Employee>>($"{_customerIdService.GetCustomerId()}/employees?{WebUtility.GetQueryString(query)}");
        }

        public async Task<IEnumerable<Unit>> SearchUnits(SearchQueryUnit searchQuery)
        {
            return await Request<IEnumerable<Unit>>($"{_customerIdService.GetCustomerId()}/units?{WebUtility.GetQueryString(searchQuery)}");
        }

        public async Task<Unit> GetUnit(int id)
        {
            return await Request<Unit>($"{_customerIdService.GetCustomerId()}/units/{id}");
        }
        public async Task<IEnumerable<ICode>> GetAbsenceTypes(SearchQueryAbsenceType query)
        {
            return await Request<IEnumerable<Code>>($"{_customerIdService.GetCustomerId()}/absencestypes?{WebUtility.GetQueryString(query)}");
        }

        public async Task<IEnumerable<IUser>> SearchUser(ISearchQueryUser searchQuery)
        {
            return await Request<IEnumerable<User>>($"{_customerIdService.GetCustomerId()}/users?{WebUtility.GetQueryString(searchQuery)}");
        }

        public async Task<IUser> GetUser(int id)
        {
            return await Request<User>($"{_customerIdService.GetCustomerId()}/users/{id}");
        }

        private async Task<T> Request<T>(string url)
        {
            Exception innerException = null;
            try
            {
                var client = _httpFactory.CreateClient(Constants.API_CLIENT_NAME);
                if (_authorizationContext.IsUserContext())
                {
                    if (_httpContextAccessor?.HttpContext?.Request?.Headers?.TryGetValue("Authorization", out var authHeader) ?? false)
                    {
                        client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authHeader);
                    }
                }
                else
                {
                    client.DefaultRequestHeaders.Add(AuthConstants.SystemUserSecret, _configuration.GetValue<string>(AuthConstants.SystemUserSecret));
                }

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception e)
            {
                innerException = e;
            }

            throw new Exception("Error requesting Common-Api", innerException);
        }       
    }
}
