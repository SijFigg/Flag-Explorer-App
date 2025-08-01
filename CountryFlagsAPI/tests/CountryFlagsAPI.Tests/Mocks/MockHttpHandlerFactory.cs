using System.Net;
using System.Text.Json;

namespace CountryFlagsAPI.Tests.Mocks
{
    public class MockHttpHandlerFactory
    {
        public static HttpMessageHandler MockSuccessHandler(object responseData)
        {
            return new TestHttpMessageHandler((request, cancellationToken) =>
            {
                var json = JsonSerializer.Serialize(responseData, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(json)
                };

                return Task.FromResult(response);
            });
        }

        public static HttpMessageHandler MockNotFoundHandler()
        {
            return new TestHttpMessageHandler((request, cancellationToken) =>
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                return Task.FromResult(response);
            });
        }

        public static HttpMessageHandler MockApiFailureHandler()
        {
            return new TestHttpMessageHandler((request, cancellationToken) =>
            {
                throw new HttpRequestException("API Down");
            });
        }

        public static HttpMessageHandler MockTimeoutHandler()
        {
            return new TestHttpMessageHandler((request, cancellationToken) =>
            {
                throw new HttpRequestException("Timeout");
            });
        }
    }
}
