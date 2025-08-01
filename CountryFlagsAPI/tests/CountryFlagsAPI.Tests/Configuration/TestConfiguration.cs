using CountryFlagsAPI.DTO;
using CountryFlagsAPI.Models;

namespace CountryFlagsAPI.Tests.Configuration
{
    public class TestConfiguration
    {

        public TestConfiguration()
        {

            Country = new CountryDto
            {
                Name = "Republic of Test",
                Capital = "Test City",
                Population = 1000000,
                Flag = "https://example.com/flag.svg"
            };

            Countries = new List<CountryDto>
        {
            new CountryDto
            {
                Name = "Testland",
                Capital = "Test City",
                Population = 5000000,
                Flag = "https://example.com/testland-flag.svg"
            },
            new CountryDto
            {
                Name = "Sample Country",
                Capital = "Sample City",
                Population = 2000000,
                Flag = "https://example.com/sample-flag.svg"
            }
        };
            RestCountries = new List<RestCountriesDto>
            {
                new RestCountriesDto
                {
                    Name = new NameDto { Common = "Testland" },
                    Capital = new List<string> { "Test City" },
                    Population = 5000000,
                    Flags = new FlagsDto { Svg = "https://example.com/testland-flag.svg" }
                },
                new RestCountriesDto
                {
                    Name = new NameDto { Common = "Sample Country" },
                    Capital = new List<string> { "Sample City" },
                    Population = 2000000,
                    Flags = new FlagsDto { Svg = "https://example.com/sample-flag.svg" }
                }
            };

            RestCountry = new RestCountriesDto
            {
                Name = new NameDto { Common = "South Africa" },
                Capital = new List<string> { "Pretoria" },
                Population = 100000,
                Flags = new FlagsDto { Svg = "https://example.com/unknown-flag.svg" }
            };

        }

        public CountryDto Country { get; set; }

        public List<CountryDto> Countries { get; set; }

        public List<RestCountriesDto> RestCountries { get; set; }

        public RestCountriesDto RestCountry { get; set; }

    }
}
