using CountryFlagsAPI.Controllers;
using CountryFlagsAPI.DTO;
using CountryFlagsAPI.Tests.Configuration;
using CountryFlagsAPI.Tests.Mocks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace CountryFlagsAPI.Tests.Unit
{

    [TestFixture]
    internal class CountryControllerTests : TestBase
    {
        private CountryController _controller;

        [SetUp]
        public new void Setup()
        {
            _controller = new CountryController(CountryService, CountryControllerLogger);
        }


        [Test]
        public async Task Given_ExistingCountries_When_GetAllCountriesIsCalled_Then_ReturnsCountries()
        {
            // Arrange
            var config = new TestConfiguration();
            CountryServiceMock.MockGetAllCountriesAsync(CountryService, config);

            // Act
            var result = await _controller.GetAllCountriesAsync();
            var okResult = result.Result as OkObjectResult;
            var countries = okResult?.Value as IEnumerable<CountryDto>;
            var countriesList = countries?.ToList();

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>();
                countriesList.Should().HaveCount(config.Countries.Count);
                countriesList.Should().BeEquivalentTo(config.Countries, options => options.WithStrictOrdering().ComparingByMembers<CountryDto>());
            }
        }

        [Test]
        public async Task Given_ExistingCountry_When_GetCountryByNameIsCalled_Then_ReturnsCountry()
        {
            // Arrange
            var config = new TestConfiguration();
            var expected = config.Country;
            var name = expected.Name;

            CountryServiceMock.MockGetCountryByNameAsync(CountryService, config);

            // Act
            var result = await _controller.GetCountryByNameAsync(name);
            var okResult = result.Result as OkObjectResult;
            var country = okResult?.Value as CountryDto;

            // Assert
            using (new AssertionScope())
            {
                result.Result.Should().BeOfType<OkObjectResult>();
                country.Should().NotBeNull();
                country.Name.Should().Be(expected.Name);
            }
        }

        [Test]
        public async Task Given_NoCountriesExist_When_GetAllCountriesIsCalled_Then_ReturnsNotFound()
        {
            // Arrange
            CountryService.GetAllCountriesAsync().Returns(new List<CountryDto>());

            // Act
            var result = await _controller.GetAllCountriesAsync();

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task Given_NullReturned_When_GetAllCountriesIsCalled_Then_ReturnsNotFound()
        {
            //Arrange
            CountryService.GetAllCountriesAsync().Returns((IEnumerable<CountryDto>?)null);

            // Act
            var result = await _controller.GetAllCountriesAsync();

            // Assert
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Test]
        public async Task Given_ServiceThrowsException_When_GetAllCountriesIsCalled_Then_ReturnsInternalServerError()
        {
            // Arrange
            CountryService.GetAllCountriesAsync().Throws(new Exception("Test error"));

            // Act
            var result = await _controller.GetAllCountriesAsync();

            // Assert
            result.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }

        [Test]
        public async Task Given_CountryDoesNotExist_When_GetCountryByNameIsCalled_Then_ReturnsNotFound()
        {
            CountryService.GetCountryByNameAsync("Atlantis").Returns((CountryDto?)null);

            var result = await _controller.GetCountryByNameAsync("Atlantis");

            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }


        [Test]
        public async Task Given_ServiceThrowsException_When_GetCountryByNameIsCalled_Then_ReturnsInternalServerError()
        {
            CountryService.GetCountryByNameAsync("Germany").Throws(new Exception("Service failure"));

            var result = await _controller.GetCountryByNameAsync("Germany");

            result.Result.Should().BeOfType<ObjectResult>()
                .Which.StatusCode.Should().Be(500);
        }



    }
}
