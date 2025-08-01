using CountryFlagsAPI.DTO;
using CountryFlagsAPI.Models;
using System.Text.Json;
using System.Web;

namespace CountryFlagsAPI.Services
{
    public class CountryService : ICountryService
    {
        private readonly ILogger<CountryService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;
        private const string ApiBaseUrl = "https://restcountries.com/v3.1/";

        public CountryService(IHttpClientFactory httpClientFactory, ILogger<CountryService> logger, JsonSerializerOptions jsonOptions)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _client = _httpClientFactory.CreateClient("CountryApi");
            _jsonOptions = jsonOptions;
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync()
        {
            _logger.LogInformation("Fetching all countries");
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    { "fields", "name,population,capital,flags" }
                };

                var builder = new UriBuilder($"{ApiBaseUrl}all");
                var query = HttpUtility.ParseQueryString(builder.Query);

                foreach (var param in parameters)
                {
                    query[param.Key] = param.Value;
                }

                builder.Query = query.ToString();
                var requestUri = builder.ToString();
                var response = await _client.GetAsync(requestUri);


                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch countries: {StatusCode}", response.StatusCode);
                    return Enumerable.Empty<CountryDto>();
                }
                var json = await response.Content.ReadAsStringAsync();
                var countriesData = JsonSerializer.Deserialize<IEnumerable<RestCountriesDto>>(json, _jsonOptions ?? new());
                return countriesData.Select(c => new CountryDto
                {
                    Name = c.Name?.Common ?? "Utopia?",
                    Capital = c.Capital?.FirstOrDefault() ?? "N/A",
                    Population = c.Population,
                    Flag = c.Flags?.Svg ?? string.Empty
                });
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching countries");
                return Enumerable.Empty<CountryDto>();
            }
        }

        public async Task<CountryDto?> GetCountryByNameAsync(string name)
        {
            _logger.LogInformation("Fetching country by name: {Name}", name);
            try
            {
                var response = await _client.GetAsync($"{ApiBaseUrl}name/{name}?fullText=true");
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to fetch country: {StatusCode}", response.StatusCode);
                    return null;
                }
                var json = await response.Content.ReadAsStringAsync();
                var countriesData = JsonSerializer.Deserialize<IEnumerable<RestCountriesDto>>(json, _jsonOptions ?? new());
                
                var countryData = countriesData?.FirstOrDefault();
                if (countryData == null)
                {
                    return null;
                }
                return new CountryDto
                {
                    Name = countryData.Name?.Common ?? "Utopia?",
                    Capital = countryData.Capital?.FirstOrDefault() ?? "N/A",
                    Population = countryData.Population,
                    Flag = countryData.Flags?.Svg ?? string.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the country by name");
                return null;
            }
        }
    }
}
