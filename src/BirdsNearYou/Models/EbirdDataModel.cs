using Newtonsoft.Json;

namespace BirdsNearYou.Models
{
    /// <summary>
    /// Used to directly bind the Ebird API data to a model.
    /// </summary>
    public class EbirdDataModel
    {
        [JsonProperty("speciesCode")]
        public string SpeciesCode { get; set; }

        [JsonProperty("comName")]
        public string ComName { get; set; }

        [JsonProperty("sciName")]
        public string SciName { get; set; }

        [JsonProperty("locId")]
        public string LocId { get; set; }

        [JsonProperty("locName")]
        public string LocName { get; set; }

        [JsonProperty("obsDt")]
        public DateTime ObsDt { get; set; }

        [JsonProperty("howMany")]
        public int HowMany { get; set; }

        [JsonProperty("lat")]
        public decimal Lat { get; set; }

        [JsonProperty("lng")]
        public decimal Lng { get; set; }

        [JsonProperty("obsValid")]
        public bool ObsValid { get; set; }

        [JsonProperty("obsReviewed")]
        public bool ObsReviewed { get; set; }

        [JsonProperty("locationPrivate")]
        public bool LocationPrivate { get; set; }

        [JsonProperty("subId")]
        public string SubId { get; set; }
    }
}
