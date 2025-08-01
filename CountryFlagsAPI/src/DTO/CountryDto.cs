namespace CountryFlagsAPI.DTO
{
    public class CountryDto
    {
        public required String? Name { get; set; }

        public required String? Capital { get; set; }

        public int? Population { get; set; }

        public required String? Flag { get; set; }
    }
}
