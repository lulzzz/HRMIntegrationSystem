using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

using Altinn.Api.Client.Models;
using System.Xml.Linq;

namespace Altinn.Api.Client.HttpClients
{
    public class AltinnClient : IAltinnClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _addSection;
        private readonly string _baseUrl;

        public AltinnClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _addSection = configuration.GetSection("Altinn.External.Api");

            _baseUrl = _addSection.GetValue<string>(Constants.Constants.API.API_URL_CONFIG_KEY);
            var apiKey = _addSection.GetValue<string>(Constants.Constants.API.API_COMPANYKEY_CONFIG_KEY);

            _client.BaseAddress = new Uri(_baseUrl);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/hal+json"));
            _client.DefaultRequestHeaders.Add("ApiKey", apiKey);
        }

        private bool _isSignedIn = false;

        private async Task SignIn()
        {
            if (_isSignedIn) return;

            var body = new
            {
                UserName = _addSection.GetValue<string>(Constants.Constants.API.API_USERNAME_CONFIG_KEY),
                UserPassword = _addSection.GetValue<string>(Constants.Constants.API.API_PASSWORD_CONFIG_KEY)
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/hal+json");
            var loginEndpoint = Path.Combine(_baseUrl, "api/authentication/authenticatewithpassword?ForceEIAuthentication");

            var responseMessage = await _client.PostAsync(loginEndpoint, content);

            _isSignedIn = responseMessage.IsSuccessStatusCode;
        }

        public Task<IEnumerable<Message>> GetMessages(string reporteeId)
        {
            var url = $"api/{reporteeId}/messages?$filter=ServiceCode {WebUtility.UrlEncode("eq '4503'")}";
            return WrappedRequest<MessageWrapper<Message>, Message>(url);
        }

        public Task<IEnumerable<Attachment>> GetAttachments(string reporteeId, string messageId)
        {
            var url = $"api/{reporteeId}/messages/{messageId}/attachments";
            return WrappedRequest<AttachmentWrapper<Attachment>, Attachment>(url);
        }

        public Task<XDocument> GetAttachmentData(string reporteeId, string messageId, string attachmentId)
        {
            var url = $"api/{reporteeId}/messages/{messageId}/attachments/{attachmentId}";
            return RequestXml(url);
        }

        public Task<IEnumerable<Reportee>> GetReportees()
        {
            var url = $"api/reportees";
            return WrappedRequest<ReporteeWrapper<Reportee>, Reportee>(url);
        }

        private async Task<IEnumerable<TModel>> WrappedRequest<TWrapper, TModel>(string url) where TWrapper : ItemWrapper<TModel>
        {
            var jsonResponse = await Request(url);
            var response = JsonConvert.DeserializeObject<EmbeddedWrapper<TWrapper>>(jsonResponse);
            return response?.Embedded?.Items;
        }

        private async Task<string> Request(string url)
        {
            if (!_isSignedIn)
            {
                await SignIn();
            }

            try
            {
                var fullUrl = Path.Combine(_baseUrl, url);
                var response = await _client.GetAsync(fullUrl);
                var json = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return json;
                }

                throw new Exception($"StatusCode: {(int)response.StatusCode} {response.StatusCode.ToString()}. {json}");
            }
            catch (HttpRequestException hre)
            {
                throw new Exception($"Error requesting Altinn-Api", hre);
            }
        }

        private async Task<XDocument> RequestXml(string url)
        {
            var jsonData = await Request(url);
            TryParseXml(jsonData, out XDocument xmlDocument);
            return xmlDocument;

        }

        private bool TryParseXml(string xml, out XDocument xmlDocument)
        {
            try
            {
                var tr = new StringReader(xml);
                var doc = XDocument.Load(tr);
                xmlDocument = doc;
                return true;
            }
            catch
            {
                xmlDocument = null;
                return false;
            }
        }
    }
}
