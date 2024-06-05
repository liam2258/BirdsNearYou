namespace BirdsNearYou.Models
{
    /// <summary>
    /// A model of the complete data passed to the MapPartial view.
    /// </summary>
    public class MapDataModel
    {
        public List<BirdDataModel> BirdData { get; set; }
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }
    }
}
