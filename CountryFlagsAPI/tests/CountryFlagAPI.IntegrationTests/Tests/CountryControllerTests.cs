using CountryFlagAPI.IntegrationTests.Configuration;
using CountryFlagsAPI.DTO;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace CountryFlagAPI.IntegrationTests.Tests
{
    internal class CountryControllerTests : TestBase
    {        

        [Test]
        public async Task GetCountryByName_ShouldReturnGermany()
        {
            var mockGermany = new[]
            {
            new
            {
                name = new { common = "Germany", official = "Federal Republic of Germany" },
                capital = new[] { "Berlin" },
                population = 83000000,
                flags = new { png = "https://flagcdn.com/de.png" }
            }
        };

            using var setupClient = new HttpClient { BaseAddress = new Uri($"http://localhost:{_mockPort}") };
            await setupClient.PutAsJsonAsync("/mockserver/expectation", new
            {
                httpRequest = new
                {
                    method = "GET",
                    path = "/name/germany"
                },
                httpResponse = new
                {
                    statusCode = 200,
                    body = JsonSerializer.Serialize(mockGermany),
                    headers = new[] { new { name = "Content-Type", values = new[] { "application/json" } } }
                }
            });

            var response = await _client.GetAsync("/api/countries/name/germany");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<CountryDto>();
            result.Name.Should().Be("Germany");
            result.Capital.Should().Be("Berlin");
            result.Flag.Should().Be("https://flagcdn.com/de.png");
        }
    }
}
