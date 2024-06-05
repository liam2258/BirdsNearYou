using BirdsNearYou.Models;
using Newtonsoft.Json;

namespace BirdsNearYou.Services
{
    public class EbirdService : IEbirdService
    {
        private readonly HttpClient _httpClient;

        public EbirdService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<EbirdDataModel>> GetBirdDataFromApiAsync(string state, string apiKey)
        {
            try
            {
                string apiUrl = $"https://api.ebird.org/v2/data/obs/US-{state}/recent?key={apiKey}";
                var response = await _httpClient.GetStringAsync(apiUrl);
                var birdData = JsonConvert.DeserializeObject<List<EbirdDataModel>>(response);
                return birdData;
            }
            catch (HttpRequestException e)
            {
                
                return null;
            }
        }
    }
}
