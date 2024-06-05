using System.Text.Json;

namespace BirdsNearYou.Services
{
    public class BirdImageService : IBirdImageService
    {
        private readonly HttpClient _httpClient;

        public BirdImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetBirdImage(string name, string apiKey)
        {
            string apiUrl = $"https://api.flickr.com/services/rest/?method=flickr.photos.search&api_key={apiKey}&format=json&nojsoncallback=1&text={name}&per_page=1&page=1";
            string imageUrl = null;

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    using (JsonDocument document = JsonDocument.Parse(json))
                    {
                        Console.WriteLine("JSON Response: " + json);
                        JsonElement root = document.RootElement;

                        if (root.GetProperty("stat").GetString() == "ok" &&
                            root.GetProperty("photos").GetProperty("photo").GetArrayLength() > 0)
                        {
                            JsonElement photo = root.GetProperty("photos").GetProperty("photo")[0];
                            int farm = photo.GetProperty("farm").GetInt32();
                            string server = photo.GetProperty("server").GetString();
                            string id = photo.GetProperty("id").GetString();
                            string secret = photo.GetProperty("secret").GetString();

                            imageUrl = $"https://farm{farm}.staticflickr.com/{server}/{id}_{secret}.jpg";
                            Console.WriteLine("ImageURL: " + imageUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error fetching bird image: {ex.Message}");
            }

            return imageUrl;
        }
    }
}
