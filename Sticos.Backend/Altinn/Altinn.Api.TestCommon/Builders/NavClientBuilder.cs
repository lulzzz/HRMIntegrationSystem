using System.Net.Http;
using Altinn.Api.Domain.Interfaces;
using FakeItEasy;

namespace Altinn.Api.TestCommon.Builders
{
    public class NavClientBuilder
    {
        private IHttpClientFactory _httpClientFactory = A.Fake<IHttpClientFactory>();
        private IXmlSerializer _xmlSerializer = A.Fake<IXmlSerializer>();

        public NavClientBuilder WithHttpClient(string clientName, HttpClient httpClient)
        {
            A.CallTo(() => _httpClientFactory.CreateClient(clientName)).Returns(httpClient);
            return this;
        }

        public NavClientBuilder WithXmlSerializer(IXmlSerializer xmlSerializer)
        {
            _xmlSerializer = xmlSerializer;
            return this;
        }

        //public IAltinnClient Build()
        //{
        //    return new AltinnClient(_httpClientFactory, _xmlSerializer);

        //}
    }
}