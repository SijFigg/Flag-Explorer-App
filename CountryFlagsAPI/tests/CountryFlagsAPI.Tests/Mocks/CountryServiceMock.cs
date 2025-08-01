using CountryFlagsAPI.Services;
using CountryFlagsAPI.Tests.Configuration;
using NSubstitute;

namespace CountryFlagsAPI.Tests.Mocks
{
    public class CountryServiceMock
    {
        public static void MockGetAllCountriesAsync(ICountryService countryService, TestConfiguration config)
        {
            countryService.GetAllCountriesAsync().Returns(config.Countries);
        }

        public static void MockGetCountryByNameAsync(ICountryService countryService, TestConfiguration config)
        {
            countryService.GetCountryByNameAsync(Arg.Any<string>()).Returns(config.Country);
        }
    }
}