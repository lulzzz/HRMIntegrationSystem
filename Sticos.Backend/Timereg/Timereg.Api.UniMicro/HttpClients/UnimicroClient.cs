using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared.Services.Extensions;
using Timereg.Api.Domain.Exceptions;
using Timereg.Api.Unimicro.Models;

namespace Timereg.Api.Unimicro.HttpClients
{
    public class UnimicroClient : IUnimicroClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private Login _loginInfo;
        private Company _company;


        public UnimicroClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
            var baseUrl = configuration.GetValueNotNull<string>(Constants.Constants.API.API_URL_CONFIG_KEY);
            _client.BaseAddress = new Uri(baseUrl);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Login> SignIn()
        {
            if (IsSignedIn)
            {
                return _loginInfo;
            }

            var body = new
            {
                UserName = _configuration.GetValue<string>(Constants.Constants.API.API_USERNAME_CONFIG_KEY),
                Password = _configuration.GetValue<string>(Constants.Constants.API.API_PASSWORD_CONFIG_KEY),
            };
            var bodyJson = JsonConvert.SerializeObject(body);
            var postRequest = new StringContent(bodyJson, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("init/sign-in", postRequest);
            var content = await result.Content.ReadAsStringAsync();

            _loginInfo = JsonConvert.DeserializeObject<Login>(content);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _loginInfo.AccessToken);
            
            return _loginInfo;
        }

        public async Task<Company> GetAndSetCompanyAuthorizationInfo(string organizationNumber)
        {
            if (string.IsNullOrWhiteSpace(organizationNumber))
            {
                throw new ApplicationException($"OrganizationNumber must be set to be used for Authorization with Unimicro-api");
            }
            if (_company?.OrganizationNumber == organizationNumber)
            {
                return _company;
            }

            var companies = await Request<IEnumerable<Company>>($"init/companies?filter=OrganizationNumber={organizationNumber}");
            _company = companies.FirstOrDefault(c=>c.OrganizationNumber==organizationNumber);
            if (_company != null)
            {
                _client.DefaultRequestHeaders.Remove(Constants.Constants.API.API_HEADER_COMPANYKEY);
                _client.DefaultRequestHeaders.Add(Constants.Constants.API.API_HEADER_COMPANYKEY, _company.Key);
            }

            return _company;
        }

        public Task<IEnumerable<SubEntity>> GetSubEntities()
        {
            return Request<IEnumerable<SubEntity>>("biz/subentities?filter=SuperiorOrganizationID gt 0&expand=BusinessRelationInfo");
        }

        public Task<IEnumerable<Employee>> GetEmployees(string organizationNumber)
        {
            var filter = $"SubEntity.OrgNumber eq {organizationNumber}";
            return Request<IEnumerable<Employee>>($"biz/employees?filter={filter}&expand=BusinessRelationInfo.DefaultEmail,SubEntity.BusinessRelationInfo");
        }
        
        private string GetOdataOrFilter(string propertyName, IEnumerable<int> listOfIds)
        {
            var ids = listOfIds.Where(i => i > 0).Distinct().ToList();
            var orFilter = "";

            if (ids.Count <= 0)
                return "";

            for (int i = 0; i < ids.Count; i++)
            {
                if (i > 0) orFilter += " or ";
                orFilter += $"{propertyName} eq {ids.ElementAt(i)}";
            }
            return $"({orFilter})";
        }

        public Task<IEnumerable<Employment>> GetEmployments(IEnumerable<int> employeeIds)
        {
            string filter = GetOdataOrFilter("EmployeeID", employeeIds);
            return Request<IEnumerable<Employment>>($"biz/employments?filter={filter}");
        }

        public Task<IEnumerable<EmploymentLeave>> GetEmploymentLeaves(IEnumerable<int> employmentIds)
        {
            string filter = GetOdataOrFilter("EmploymentID", employmentIds);
            return Request<IEnumerable<EmploymentLeave>>($"biz/EmployeeLeave?filter={filter}");
        }

        public async Task<int> PostEmploymentLeave(EmploymentLeave employmentLeave)
        {
            var createdEmploymentLeave = await Post<EmploymentLeave, EmploymentLeave>("biz/EmployeeLeave", employmentLeave);
            return createdEmploymentLeave.Id;
        }

        public async Task DeleteEmploymentLeave(string employmentLeaveId)
        {
            await Delete($"biz/EmployeeLeave/{employmentLeaveId}");
        }
        
        public async Task<EmploymentLeave> PutEmploymentLeave(EmploymentLeave employmentLeave)
        {
            return await Put<EmploymentLeave, EmploymentLeave>($"biz/employeeLeave{employmentLeave.Id}", employmentLeave);
        }

        public Task<IEnumerable<Worker>> GetWorkers(IEnumerable<int> employeeIds)
        {
            string filter = GetOdataOrFilter("EmployeeId", employeeIds);
            return Request<IEnumerable<Worker>>($"biz/workers/?filter={filter}&expand=Info&expand=Relations&hateoas=true");
        }

        public Task<IEnumerable<User>> GetUsers(IEnumerable<int> userIds)
        {
            string filter = GetOdataOrFilter("Id", userIds);
            return Request<IEnumerable<User>>($"biz/Users/?filter={filter}");
        }

        public Task<IEnumerable<WorkType>> GetWorkTypes()
        {
            return Request<IEnumerable<WorkType>>("biz/worktypes/?hateoas=false");
        }

        public Task<IEnumerable<WorkRelation>> GetWorkRelations(IEnumerable<int> workerIds)
        {
            string filter = GetOdataOrFilter("WorkerId", workerIds);
            return Request<IEnumerable<WorkRelation>>($"biz/workrelations?expand=workprofile&filter={filter}");
        }
        
        public async Task<WorkItem> GetWorkItem(int workItemId)
        {
            return await Request<WorkItem>($"biz/workitems/{workItemId}");
        }
        
        public async Task<int> PostWorkItem(WorkItem workItem)
        {
            var createdWorkItem = await Post<WorkItem, WorkItem>("biz/workitems", workItem);
            return createdWorkItem.Id;
        }

        public async Task DeleteWorkItem(string workItemId)
        {
            await Delete($"biz/workitems/{workItemId}");
        }

        public async Task<WorkItem> PutWorkItem(WorkItem workItem)
        {
            return await Put<WorkItem, WorkItem>($"biz/workitems/{workItem.Id}", workItem);
        }

        public Task<HourBalance> GetHourBalance(int workRelationId)
        {
            return Request<HourBalance>($"biz/workrelations/{workRelationId}?action=calc-flex-balance&hateoas=false");
        }
        
        private async Task<T> Request<T>(string url)
        {
            if (!IsSignedIn) { throw new Exception("Httpclient must be signed in first"); }

            try
            {
                var response = await _client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }

                throw new ExternalSystemCommunicationException($"StatusCode: {(int)response.StatusCode} {response.StatusCode.ToString()}. {json}");
            }
            catch (HttpRequestException hre)
            {
                throw new ExternalSystemCommunicationException($"Error requesting Unimicro-Api", hre);
            }
        }
        private async Task Delete(string url)
        {
            if (!IsSignedIn) { throw new Exception("Httpclient must be signed in first"); }

            try
            {
                var response = await _client.DeleteAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                throw new ExternalSystemCommunicationException($"StatusCode: {(int)response.StatusCode} {response.StatusCode.ToString()}. {json}");
            }
            catch (HttpRequestException hre)
            {
                throw new ExternalSystemCommunicationException($"Error deleting Unimicro-Api", hre);
            }
        }
        private async Task<TOut> Post<TIn,TOut>(string url, TIn payload)
        {
            if (!IsSignedIn) { throw new Exception("Httpclient must be signed in first"); }

            try
            {
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var stringContentPayload = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var createdResponse = await _client.PostAsync(url, stringContentPayload);
                var jsonResponse = await createdResponse.Content.ReadAsStringAsync();
                if (createdResponse.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TOut>(jsonResponse);
                }

                throw new ExternalSystemCommunicationException($"StatusCode: {(int)createdResponse.StatusCode} {createdResponse.StatusCode.ToString()}. {jsonResponse}");
            }
            catch (HttpRequestException hre)
            {
                throw new ExternalSystemCommunicationException($"Error posting to Unimicro-Api", hre);
            }
        }
        private async Task<TOut> Put<TIn, TOut>(string url, TIn payload)
        {
            if (!IsSignedIn) { throw new Exception("Httpclient must be signed in first"); }

            try
            {
                var jsonPayload = JsonConvert.SerializeObject(payload);
                var stringContentPayload = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                var createdResponse = await _client.PutAsync(url, stringContentPayload);
                var jsonResponse = await createdResponse.Content.ReadAsStringAsync();
                if (createdResponse.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<TOut>(jsonResponse);
                }

                throw new ExternalSystemCommunicationException($"StatusCode: {(int)createdResponse.StatusCode} {createdResponse.StatusCode.ToString()}. {jsonResponse}");
            }
            catch (HttpRequestException hre)
            {
                throw new ExternalSystemCommunicationException($"Error posting to Unimicro-Api", hre);
            }
        }

        private bool IsSignedIn => _loginInfo?.AccessToken != null;
    }
}