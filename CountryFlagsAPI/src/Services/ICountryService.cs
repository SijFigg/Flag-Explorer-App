using CountryFlagsAPI.DTO;

namespace CountryFlagsAPI.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryDto>> GetAllCountriesAsync();

        Task<CountryDto?> GetCountryByNameAsync(string name);
    }
}
