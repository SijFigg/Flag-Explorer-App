import { useParams } from 'react-router-dom';
import { getCountryByName } from '../api/countryService';
import { useEffect, useState } from 'react';
import '../styles/Country.css'; 

function Country() {
    const { name } = useParams();
    const [country, setCountry] = useState(null);

    useEffect(() => {
        getCountryByName(name).then(setCountry);
    }, [name]);

    if (!country) return <p>Loading...</p>;

    return (
        <div className="country-container">
            <h2>{country.name}</h2>
            <img src={country.flag} alt={`Flag of ${country.name}`} />
            <p>Population: {country.population.toLocaleString()}</p>
            <p>Capital: {country.capital}</p>
        </div>
    );
}

export default Country;
