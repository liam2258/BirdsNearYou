namespace BirdsNearYou.Models
{
    /// <summary>
    /// Represents the data model for bird sightings that are passed to the MapPartial view.
    /// </summary>
    public class BirdDataModel
    {
        public string? Name { get; set; }

        public string? ScientificName { get; set; }

        public string? SpeciesCode { get; set; }

        public string? ImageURL { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public int? Amount { get; set; }

        public DateTime? Time { get; set; }

    }
}
