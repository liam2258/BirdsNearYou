using Moq;
using Moq.Protected;
using System.Net;
using Newtonsoft.Json;
using BirdsNearYou.Models;

namespace BirdsNearYou.Services.Tests
{
    public class EbirdServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly EbirdService _ebirdService;

        public EbirdServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _ebirdService = new EbirdService(_httpClient);
        }

        [Fact]
        public async Task GetBirdDataFromApiAsync_ReturnsData_WhenApiResponseIsSuccessful()
        {
            // Arrange
            string state = "CO";
            string apiKey = "fake-api-key";
            string apiUrl = $"https://api.ebird.org/v2/data/obs/US-{state}/recent?key={apiKey}";
            var responseData = new List<EbirdDataModel>
            {
                new EbirdDataModel { SpeciesCode = "amecro", ComName = "American Crow" },
                new EbirdDataModel { SpeciesCode = "amerob", ComName = "American Robin" }
            };
            var jsonResponse = JsonConvert.SerializeObject(responseData);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == apiUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            // Act
            var result = await _ebirdService.GetBirdDataFromApiAsync(state, apiKey);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count); // Assuming we expect 2 bird data entries
            Assert.Equal("amecro", result[0].SpeciesCode);
            Assert.Equal("American Crow", result[0].ComName);
            Assert.Equal("amerob", result[1].SpeciesCode);
            Assert.Equal("American Robin", result[1].ComName);
        }

        [Fact]
        public async Task GetBirdDataFromApiAsync_ReturnsNull_WhenApiResponseIsUnsuccessful()
        {
            // Arrange
            string state = "CO";
            string apiKey = "fake-api-key";
            string apiUrl = $"https://api.ebird.org/v2/data/obs/US-{state}/recent?key={apiKey}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == apiUrl),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act
            var result = await _ebirdService.GetBirdDataFromApiAsync(state, apiKey);

            // Assert
            Assert.Null(result);
        }
    }
}
