import { getAllCountries, getCountryByName } from '../../src/api/countries';

global.fetch = jest.fn();

afterEach(() => {
    fetch.mockClear();
});

describe('API functions', () => {
    test('getAllCountries fetches from the correct URL', async () => {
        const mockData = [{ name: 'Nigeria' }, { name: 'Kenya' }];
        fetch.mockResolvedValueOnce({
            json: async () => mockData
        });

        const result = await getAllCountries();

        expect(fetch).toHaveBeenCalledWith('http://localhost:8000/countries');
        expect(result).toEqual(mockData);
    });

    test('getCountryByName fetches with encoded country name', async () => {
        const mockData = { name: 'Côte d\'Ivoire' };
        const countryName = "Côte d'Ivoire";
        fetch.mockResolvedValueOnce({
            json: async () => mockData
        });

        const result = await getCountryByName(countryName);

        expect(fetch).toHaveBeenCalledWith(
            `http://localhost:8000/countries/${encodeURIComponent(countryName)}`
        );
        expect(result).toEqual(mockData);
    });
});
