using BirdsNearYou.Models;

namespace BirdsNearYou.Services
{
    public interface IBirdImageService
    {
        Task<string> GetBirdImage(string bird, string apiKey);
    }
}
