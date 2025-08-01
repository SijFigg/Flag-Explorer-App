import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import Home from '../../src/pages/Home';
import { getAllCountries } from '../../src/api/countryService';
import { useNavigate } from 'react-router-dom';

jest.mock('../../src/api/countryService', () => ({
    getAllCountries: jest.fn(),
}));

jest.mock('react-router-dom', () => ({
    ...jest.requireActual('react-router-dom'),
    useNavigate: jest.fn(),
}));

describe('Home Page', () => {
    const mockCountries = [
        { name: 'Ghana', flag: 'https://example.com/ghana.png' },
        { name: 'Senegal', flag: 'https://example.com/senegal.png' },
    ];

    const mockNavigate = jest.fn();

    beforeEach(() => {
        getAllCountries.mockResolvedValue(mockCountries);
        useNavigate.mockReturnValue(mockNavigate);
    });

    test('renders country cards after data fetch', async () => {
        render(<Home />);

        await waitFor(() => {
            expect(screen.getByText('Ghana')).toBeInTheDocument();
            expect(screen.getByText('Senegal')).toBeInTheDocument();
        });

        expect(screen.getAllByRole('img')).toHaveLength(2);
        expect(screen.getByAltText('Ghana Flag')).toHaveAttribute('src', mockCountries[0].flag);
    });

    test('navigates to country page on card click', async () => {
        render(<Home />);

        await waitFor(() => {
            expect(screen.getByText('Ghana')).toBeInTheDocument();
        });

        fireEvent.click(screen.getByText('Ghana'));

        expect(mockNavigate).toHaveBeenCalledWith('/country/Ghana');
    });
});
