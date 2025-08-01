import '../styles/CountryCard.css'

function CountryCard({ name, flag, onClick }) {
    return (
        <div className="country-card" onClick={onClick}>
            <img src={flag} alt={`${name} Flag`} />
            <p>{name}</p>
        </div>
    );
    console.log("Flag URL:", flag);
}

export default CountryCard;
