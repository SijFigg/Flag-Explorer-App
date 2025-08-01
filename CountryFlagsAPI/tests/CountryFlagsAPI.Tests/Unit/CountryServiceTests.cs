using CountryFlagsAPI.Services;
using CountryFlagsAPI.Tests.Configuration;
using CountryFlagsAPI.Tests.Mocks;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;

namespace CountryFlagsAPI.Tests.Unit
{
    [TestFixture]
    internal class CountryServiceTests : TestBase
    {        
        private TestConfiguration _config;
        

        [SetUp]
        public void SetUp()
        {
            _config = new TestConfiguration();                        
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldReturnExpectedList_WhenApiSucceeds()
        {
            //Arrange            
            HttpMessageHandler = MockHttpHandlerFactory.MockSuccessHandler(_config.RestCountries);

            var httpClient = new HttpClient(HttpMessageHandler)
            {
                BaseAddress = new Uri("https://TestUrl.com/")
            };

            HttpClientFactory.CreateClient("CountryApi").Returns(httpClient);
            var service = new CountryService(HttpClientFactory, CountryServiceLogger, JsonOptions);

            // Act
            var result = (await service.GetAllCountriesAsync()).ToList();

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result.Should().HaveSameCount(_config.Countries);

                for (int i = 0; i < result.Count; i++)
                {
                    result[i].Name.Should().Be(_config.Countries[i].Name);
                    result[i].Capital.Should().Be(_config.Countries[i].Capital);
                    result[i].Population.Should().Be(_config.Countries[i].Population);
                    result[i].Flag.Should().Be(_config.Countries[i].Flag);
                }
            }

            httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldReturnEmpty_WhenApiReturnsFailureStatus()
        {
            // Arrange
            HttpMessageHandler = MockHttpHandlerFactory.MockApiFailureHandler();
            var httpClient = new HttpClient(HttpMessageHandler)

            {
                BaseAddress = new Uri("https://Countries.com/")
            };
            HttpClientFactory.CreateClient("CountryApi").Returns(httpClient);
            var service = new CountryService(HttpClientFactory, CountryServiceLogger, JsonOptions);

            // Act
            var result = (await service.GetAllCountriesAsync()).ToList();

            // Assert
            result.Should().BeEmpty();

            httpClient?.Dispose();
        }

        [Test]
        public async Task GetCountryByNameAsync_ShouldReturnMatchingCountry_WhenValidNameIsGiven()
        {
            // Arrange
            HttpMessageHandler = MockHttpHandlerFactory.MockSuccessHandler(new[] { _config.RestCountry });

            var httpClient = new HttpClient(HttpMessageHandler)
            {
                BaseAddress = new Uri("https://Countries.com/")
            };
            HttpClientFactory.CreateClient("CountryApi").Returns(httpClient);
            var service = new CountryService(HttpClientFactory, CountryServiceLogger, JsonOptions);

            var countryName = _config.Country.Name;

            // Act
            var result = await service.GetCountryByNameAsync(countryName);

            // Assert
            using (new AssertionScope())
            {
                result.Should().NotBeNull();
                result!.Name.Should().Be(_config.RestCountry.Name.Common);
                result.Capital.Should().Be(_config.RestCountry.Capital[0]);
                result.Population.Should().Be(_config.RestCountry.Population);
                result.Flag.Should().Be(_config.RestCountry.Flags.Svg);
            }

            httpClient?.Dispose();
        }

        [Test]
        public async Task GetCountryByNameAsync_ShouldReturnNull_WhenApiReturnsNotFound()
        {
            // Arrange
            HttpMessageHandler = MockHttpHandlerFactory.MockNotFoundHandler();

            var httpClient = new HttpClient(HttpMessageHandler)
            {
                BaseAddress = new Uri("https://Countries.com/")
            };
            HttpClientFactory.CreateClient("CountryApi").Returns(httpClient);
            var service = new CountryService(HttpClientFactory, CountryServiceLogger, JsonOptions);

            // Act
            var result = await service.GetCountryByNameAsync("NonExistentCountry");

            // Assert
            result.Should().BeNull();

            httpClient?.Dispose();
        }

        [Test]
        public async Task GetCountryByNameAsync_ShouldReturnNull_WhenApiThrowsException()
        {
            // Arrange
            HttpMessageHandler = MockHttpHandlerFactory.MockApiFailureHandler();

            var httpClient = new HttpClient(HttpMessageHandler)
            {
                BaseAddress = new Uri("https://Countries.com/")
            };
            HttpClientFactory.CreateClient("CountryApi").Returns(httpClient);
            var service = new CountryService(HttpClientFactory, CountryServiceLogger, JsonOptions);

            // Act
            var result = await service.GetCountryByNameAsync("France");

            // Assert
            result.Should().BeNull();

            httpClient?.Dispose();
        }

        [Test]
        public async Task GetAllCountriesAsync_ShouldReturnEmpty_WhenApiThrowsException()
        {
            // Arrange
            HttpMessageHandler = MockHttpHandlerFactory.MockTimeoutHandler();

            var httpClient = new HttpClient(HttpMessageHandler)
            {
                BaseAddress = new Uri("https://Countries.com/")
            };
            HttpClientFactory.CreateClient("CountryApi").Returns(httpClient);
            var service = new CountryService(HttpClientFactory, CountryServiceLogger, JsonOptions);

            // Act
            var result = (await service.GetAllCountriesAsync()).ToList();

            // Assert
            result.Should().BeEmpty();

            httpClient?.Dispose();
        }
     
    }
}
