using BirdsNearYou.Models;
using BirdsNearYou.Services;
using Microsoft.AspNetCore.Mvc;

namespace BirdsNearYou.Controllers
{
    /// <summary>
    /// Controller responsible for handling map-related actions.
    /// Inlcudes the process of abstracting data from the eBird API and the Flickr API
    /// to pass to the MapPartial view.
    /// </summary>
    public class MapController : Controller
    {
        private readonly IEbirdService _ebirdService;
        private readonly IBirdImageService _birdImageService;

        public MapController(IEbirdService ebirdService, IBirdImageService birdImageService)
        {
            _ebirdService = ebirdService;
            _birdImageService = birdImageService;
        }

        [HttpPost]
        public async Task<IActionResult> GetBirdMap(string state, decimal latitude, decimal longitude)
        {
            string? secret = System.Environment.GetEnvironmentVariable("EBIRD_API_KEY");
            string? secret2 = System.Environment.GetEnvironmentVariable("FLICKR_API_KEY");

            // Get bird data from eBird API
            List<EbirdDataModel> ebirdList = await _ebirdService.GetBirdDataFromApiAsync(state, secret);
            Console.WriteLine(ebirdList.Count);

            var birdList = ebirdList
                .GroupBy(e => new { e.Lat, e.Lng })
                .Select(g => g.First())
                .Take(5)
                .Select(ebirdData => new BirdDataModel
                {
                    Name = ebirdData.ComName,
                    ScientificName = ebirdData.SciName,
                    SpeciesCode = ebirdData.SpeciesCode,
                    Latitude = ebirdData.Lat,
                    Longitude = ebirdData.Lng,
                    Amount = ebirdData.HowMany,
                    Time = ebirdData.ObsDt
                })
                .ToList();

            // Fetch bird images asynchronously
            foreach (var bird in birdList)
            {
                bird.ImageURL = await _birdImageService.GetBirdImage(bird.ScientificName, secret2);
            }

            if (birdList == null)
            {
                return PartialView("_MapPartial", new List<BirdDataModel>());
            }

            MapDataModel mapData = new MapDataModel
            {
                BirdData = birdList,
                Longitude = longitude,
                Latitude = latitude
            };

            return PartialView("_MapPartial", mapData);
        }
    }
}
