namespace CountryFlagsAPI.Models
{
    public class RestCountriesDto
    {
        public NameDto? Name { get; set; }
        public List<string>? Capital { get; set; }
        public string? Region { get; set; }
        public string? Subregion { get; set; }
        public int? Population { get; set; }
        public List<string>? Timezones { get; set; }
        public Dictionary<string, CurrencyDto>? Currencies { get; set; }
        public FlagsDto? Flags { get; set; }
        public CoatOfArmsDto? CoatOfArms { get; set; }
        public Dictionary<string, string>? Languages { get; set; }
        public List<string>? Borders { get; set; }
        public double[]? Latlng { get; set; }
        public string? Cca2 { get; set; }
        public string? Cca3 { get; set; }
        public string? Ccn3 { get; set; }
        public string? Cioc { get; set; }
    }
}
