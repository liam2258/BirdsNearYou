using BirdsNearYou.Models;

namespace BirdsNearYou.Services
{
    public interface IEbirdService
    {
        Task<List<EbirdDataModel>> GetBirdDataFromApiAsync(string state, string apiKey);
    }
}
