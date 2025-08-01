import { render, screen, fireEvent } from '@testing-library/react';
import CountryCard from '../../src/components/CountryCard';

describe('CountryCard', () => {
    const props = {
        name: 'Ghana',
        flag: 'https://example.com/ghana.png',
        onClick: jest.fn(),
    };

    test('renders flag image and country name', () => {
        render(<CountryCard {...props} />);

        // Check for country name
        expect(screen.getByText('Ghana')).toBeInTheDocument();

        // Check for image with correct alt and src
        const img = screen.getByAltText('Ghana Flag');
        expect(img).toBeInTheDocument();
        expect(img).toHaveAttribute('src', props.flag);
    });

    test('calls onClick when card is clicked', () => {
        render(<CountryCard {...props} />);

        const card = screen.getByText('Ghana').closest('.country-card'); 
        fireEvent.click(card);

        expect(props.onClick).toHaveBeenCalledTimes(1);
    });
});
