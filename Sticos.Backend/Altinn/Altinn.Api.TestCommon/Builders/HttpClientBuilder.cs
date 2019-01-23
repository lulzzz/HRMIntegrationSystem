using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Altinn.Api.Domain.Interfaces;
using FakeItEasy;

namespace Altinn.Api.TestCommon.Builders
{
    public class HttpClientBuilder
    {
        private HttpClientHandler _httpClientHandler = new HttpClientHandler();
        private Action<HttpClient> _config = client => {};

        public HttpClientBuilder WithCertificate(X509Certificate2 certificate)
        {
            _httpClientHandler.ClientCertificates.Add(certificate);
            return this;
        } 
        public HttpClientBuilder WithClientConfig(Action<HttpClient> config)
        {
            _config = config;
            return this;
        }

        public HttpClient Build()
        {
            var httpClient = new HttpClient(_httpClientHandler);
            _config.Invoke(httpClient);
            return httpClient;
        }
    }
}