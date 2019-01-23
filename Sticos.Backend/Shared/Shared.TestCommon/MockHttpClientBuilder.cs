using FakeItEasy;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.TestCommon
{
    public class MockHttpClientBuilder
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MockHttpClientHandler _httpClientHandler;

        public MockHttpClientBuilder(string name, string baseUrl)
        {
            _httpClientFactory = A.Fake<IHttpClientFactory>();
            _httpClientHandler = A.Fake<MockHttpClientHandler>(opt => opt.CallsBaseMethods());

            var httpClient = new HttpClient(_httpClientHandler) { BaseAddress = new Uri(baseUrl) };

            A.CallTo(() => _httpClientFactory.CreateClient(name)).Returns(httpClient);

            A.CallTo(() => _httpClientHandler.SendAsync(A<HttpMethod>.Ignored, A<string>.Ignored))
                .ReturnsLazily(() => CreateResponse("Error message", HttpStatusCode.InternalServerError));
            A.CallTo(() => _httpClientHandler.SendAsync(A<HttpMethod>.Ignored, A<string>.Ignored, A<string>.Ignored))
                .ReturnsLazily(() => CreateResponse("Error message", HttpStatusCode.InternalServerError));
        }

        public void WithExpectedGetResponse(string uri, string expectedContent, HttpStatusCode statusCode)
        {
            A.CallTo(() => _httpClientHandler.SendAsync(HttpMethod.Get, A<string>.That.EndsWith("/" + uri)))
                .ReturnsLazily(() => CreateResponse(expectedContent, statusCode));
        }

        public void WithExpectedPostResponse(string uri, string expectedContent, HttpStatusCode statusCode,
            string expectedPayload)
        {
            A.CallTo(() =>
                    _httpClientHandler.SendAsync(HttpMethod.Post, A<string>.That.EndsWith("/" + uri), expectedPayload))
                .ReturnsLazily(() => CreateResponse(expectedContent, statusCode));
        }

        private HttpResponseMessage CreateResponse(string content, HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage
            {
                Content = new StringContent(content),
                StatusCode = statusCode
            };
            return response;
        }

        public IHttpClientFactory Build()
        {
            return _httpClientFactory;
        }
    }

    public abstract class MockHttpClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Content != null)
            {
                var payload = await request.Content.ReadAsStringAsync();
                return SendAsync(request.Method, request.RequestUri.PathAndQuery, payload);
            } 
            return SendAsync(request.Method, request.RequestUri.PathAndQuery);
        }

        public abstract HttpResponseMessage SendAsync(HttpMethod method, string url, string payload);
        public abstract HttpResponseMessage SendAsync(HttpMethod method, string url);

    }
}