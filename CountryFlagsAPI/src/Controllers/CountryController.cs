using CountryFlagsAPI.DTO;
using CountryFlagsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CountryFlagsAPI.Controllers
{

    [ApiController]
    [Route("countries")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountryController> _logger;

        public CountryController(ICountryService countryService, ILogger<CountryController> logger)
        {
            _countryService = countryService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAllCountriesAsync()
        {
            _logger.LogInformation("Request received to get all countries");
            try
            {
                var countries = await _countryService.GetAllCountriesAsync();
                if (countries == null || !countries.Any())
                {
                    _logger.LogWarning("No countries found");
                    return NotFound("No countries found");
                }
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching countries");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<CountryDto>> GetCountryByNameAsync(string name)
        {
            _logger.LogInformation("Request received to get country by name: {Name}", name);
            try
            {
                var country = await _countryService.GetCountryByNameAsync(name);
                if (country == null)
                {
                    _logger.LogWarning("Country not found: {Name}", name);
                    return NotFound($"Country '{name}' not found");
                }
                return Ok(country);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the country by name: {Name}", name);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
