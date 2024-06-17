using System.Data.SqlClient;
using System.Text.Json;

namespace BirdsNearYou.Services
{
    public class BirdImageService : IBirdImageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _connectionString;

        public BirdImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        }

        public async Task<string> GetBirdImage(string name, string apiKey)
        {
            string imageUrl = await GetImageUrlFromDatabase(name);

            if (string.IsNullOrEmpty(imageUrl))
            {
                imageUrl = await GetImageUrlFromApi(name, apiKey);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    await SaveImageUrlToDatabase(name, imageUrl);
                }
            }

            return imageUrl;
        }

        private async Task<string> GetImageUrlFromDatabase(string name)
        {
            string imageUrl = null;

            string query = "SELECT url FROM BirdImages WHERE name = @name";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            imageUrl = reader["url"].ToString();
                        }
                    }
                }
            }

            return imageUrl;
        }

        private async Task<string> GetImageUrlFromApi(string name, string apiKey)
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

        private async Task SaveImageUrlToDatabase(string name, string url)
        {
            string query = "INSERT INTO BirdImages (name, url) VALUES (@name, @url)";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@url", url);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
