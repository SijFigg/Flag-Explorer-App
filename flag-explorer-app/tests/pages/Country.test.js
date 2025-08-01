import { render, screen, waitFor } from '@testing-library/react';
import Country from '../../src/pages/Country';
import { useParams } from 'react-router-dom';
import { getCountryByName } from '../../src/api/countryService';

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useParams: jest.fn(),
}));

jest.mock('../../src/api/countryService', () => ({
    getCountryByName: jest.fn(),
}));

describe('Country Page', () => {
    const mockCountry = {
        name: 'Kenya',
        flag: 'https://example.com/kenya.png',
        population: 53771300,
        capital: 'Nairobi',
    };

    beforeEach(() => {
        useParams.mockReturnValue({ name: 'Kenya' });
    });

    test('displays loading state initially', () => {
        getCountryByName.mockReturnValue(new Promise(() => { })); 
        render(<Country />);
        expect(screen.getByText(/loading/i)).toBeInTheDocument();
    });

    test('renders country details after fetch', async () => {
        getCountryByName.mockResolvedValue(mockCountry);

        render(<Country />);

        await waitFor(() => {
            expect(screen.getByText('Kenya')).toBeInTheDocument();
        });

        expect(screen.getByAltText('Flag of Kenya')).toHaveAttribute('src', mockCountry.flag);
        expect(screen.getByText(/Population:/)).toHaveTextContent('Population: 53,771,300');
        expect(screen.getByText(/Capital:/)).toHaveTextContent('Capital: Nairobi');
    });
});
