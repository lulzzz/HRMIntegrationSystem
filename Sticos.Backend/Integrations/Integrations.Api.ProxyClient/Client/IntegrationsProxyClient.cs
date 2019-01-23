using Integrations.Api.Contracts;
using Integrations.Api.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Interfaces;
using Shared.Services.Constants;
using Shared.Services.Helpers;
using Shared.Services.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Integrations.Api.ProxyClient.Client
{
    public class IntegrationsProxyClient : IIntegrationService, IEntityMapService
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationContextService _authorizationContext;
        private readonly ICustomerIdService _customerIdService;

        public IntegrationsProxyClient(IHttpClientFactory httpFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration,
            IAuthorizationContextService authorizationContext, ICustomerIdService customerIdService)
        {
            _httpFactory = httpFactory;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _authorizationContext = authorizationContext;
            _customerIdService = customerIdService;
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

            throw new Exception("Error requesting Integrations-Api", innerException);
        }

        public async Task<Integration> GetIntegration(int id)
        {
            return await Request<Integration>($"{_customerIdService.GetCustomerId()}/integrations/{id}");
        }

        public async Task<IEnumerable<Integration>> Search(SearchQueryIntegration searchQuery)
        {
            return await Request<IEnumerable<Integration>>($"{_customerIdService.GetCustomerId()}/integrations?{WebUtility.GetQueryString(searchQuery)}");
        }

        public async Task<IEnumerable<EntityMap>> SearchEntityMaps(SearchQueryEntityMap searchQuery)
        {
            return await Request<IEnumerable<EntityMap>>($"{_customerIdService.GetCustomerId()}/entitymaps?{WebUtility.GetQueryString(searchQuery)}");
        }

    }
}
