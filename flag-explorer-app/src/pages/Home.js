import { useEffect, useState } from 'react';
import { getAllCountries } from '../api/countryService';
import CountryCard from '../components/CountryCard';
import { useNavigate } from 'react-router-dom';
import '../styles/Home.css';

function Home() {
    const [countries, setCountries] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        getAllCountries().then(data => {
            console.log("Fetched countries:", data); 
            setCountries(data);
        });
    }, []);

    return (
        <div className="country-grid">
            {Array.isArray(countries) && countries.map((country) => (
                <CountryCard
                    key={country.name}
                    name={country.name}
                    flag={country.flag}
                    onClick={() => navigate(`/country/${country.name}`)}
                />
            ))}
        </div>
        
    );
}

export default Home;
