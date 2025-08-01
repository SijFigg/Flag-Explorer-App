using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using NUnit.Framework;
using System.Net.Http.Json;
using System.Text.Json;

namespace CountryFlagAPI.IntegrationTests.Configuration
{
    public class TestBase
    {
        private IContainer _mockServerContainer;
        private IContainer _apiContainer;
        protected HttpClient _client;
        protected int _mockPort;
        protected int _apiPort;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            // Start mockserver container first
            _mockServerContainer = new ContainerBuilder()
                .WithImage("jamesdbloom/mockserver:mockserver-5.11.2")
                .WithName($"mockserver-nunit-{Guid.NewGuid()}")
                .WithPortBinding(1080, assignRandomHostPort: true)
                .WithWaitStrategy(Wait.ForUnixContainer())
                .Build();

            await _mockServerContainer.StartAsync();
            _mockPort = _mockServerContainer.GetMappedPublicPort(1080);

            // Wait for MockServer to be ready
            await WaitForMockServerToBeReady();

            // Set up mock expectation
            await SetupMockExpectation();

            // Build and start the API container
            _apiContainer = new ContainerBuilder()
                .WithImage("countryflagapi:latest") // Replace with your actual image name
                .WithName($"api-nunit-{Guid.NewGuid()}")
                .WithPortBinding(80, assignRandomHostPort: true) // Assuming your API runs on port 80 in container
                .WithEnvironmentVariable("CountryApi__BaseUrl", $"http://host.docker.internal:{_mockPort}/")
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(r => r
                        .ForPath("/health") // Adjust this to your health check endpoint
                        .ForPort(80)))
                .Build();

            await _apiContainer.StartAsync();
            _apiPort = _apiContainer.GetMappedPublicPort(80);

            // Setup HttpClient for the actual test
            _client = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{_apiPort}")
            };
        }

        private async Task WaitForMockServerToBeReady()
        {
            using var client = new HttpClient();
            var maxAttempts = 30;
            var delay = TimeSpan.FromSeconds(1);

            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    // Try to access the mockserver - any response (even 404) means it's running
                    var response = await client.GetAsync($"http://localhost:{_mockPort}/");
                    // MockServer is responding, so it's ready
                    return;
                }
                catch (HttpRequestException)
                {
                    // Service not ready yet, continue waiting
                }

                await Task.Delay(delay);
            }

            throw new TimeoutException("MockServer did not start within the expected time");
        }

        private async Task SetupMockExpectation()
        {
            // Prepare mock response data
            var mockResponse = new[]
            {
                new
                {
                    name = new { common = "France", official = "French Republic" },
                    capital = new[] { "Paris" },
                    population = 67000000,
                    flags = new { png = "https://flagcdn.com/fr.png" }
                }
            };

            // Set up mock expectation
            using var setupClient = new HttpClient { BaseAddress = new Uri($"http://localhost:{_mockPort}") };
            var expectation = new
            {
                httpRequest = new
                {
                    method = "GET",
                    path = "/v3.1/all"
                },
                httpResponse = new
                {
                    statusCode = 200,
                    body = JsonSerializer.Serialize(mockResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                    headers = new[]
                    {
                        new { name = "Content-Type", values = new[] { "application/json" } }
                    }
                }
            };

            var response = await setupClient.PutAsJsonAsync("/mockserver/expectation", expectation);
            response.EnsureSuccessStatusCode();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            _client?.Dispose();

            if (_apiContainer != null)
            {
                await _apiContainer.StopAsync();
                await _apiContainer.DisposeAsync();
            }

            if (_mockServerContainer != null)
            {
                await _mockServerContainer.StopAsync();
                await _mockServerContainer.DisposeAsync();
            }
        }
    }
}