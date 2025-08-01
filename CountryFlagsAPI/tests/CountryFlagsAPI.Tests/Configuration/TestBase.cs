using CountryFlagsAPI.Controllers;
using CountryFlagsAPI.Services;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework.Internal;
using System.Net.Http;
using System.Text.Json;

namespace CountryFlagsAPI.Tests.Configuration
{
    [TestFixture]
    public class TestBase
    {
        protected ICountryService? CountryService;
        protected ILogger<CountryService> CountryServiceLogger;
        protected ILogger<CountryController> CountryControllerLogger;
        protected HttpMessageHandler HttpMessageHandler;
        protected JsonSerializerOptions JsonOptions;
        protected IHttpClientFactory HttpClientFactory;

        [OneTimeSetUp]
        public void Setup()
        {
            CountryService = Substitute.For<ICountryService>();
            CountryServiceLogger = Substitute.For<ILogger<CountryService>>();
            CountryControllerLogger = Substitute.For<ILogger<CountryController>>();
            HttpMessageHandler = Substitute.For<HttpMessageHandler>();
            HttpClientFactory = Substitute.For<IHttpClientFactory>();

            JsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        [OneTimeTearDown]
        public void GlobalTeardown()
        {
            if (HttpMessageHandler is IDisposable disposable)
            {
                disposable.Dispose();
            }

            // Add any global cleanup here if needed
            HttpClientFactory = null;
            CountryService = null;
            JsonOptions = null;
            CountryServiceLogger = null;
            CountryControllerLogger = null;
            HttpClientFactory = null;
        }

    }
}
