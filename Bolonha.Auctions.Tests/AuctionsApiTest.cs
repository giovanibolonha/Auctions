using System.Text;
using System.Text.Json;

namespace Bolonha.Auctions.Tests;

public partial class AuctionsApiTest(AuctionsApiFixture appHostFixture)
    : IClassFixture<AuctionsApiFixture>
{
    const string AuctionsApi = "/api/auctions";
    const string BidsApi = "/api/bids";

    private readonly HttpClient _httpClient = appHostFixture.CreateHttpClient() ?? throw new ApplicationException();
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    private static StringContent ConvertToContentRequest(object request)
        => new(JsonSerializer.Serialize(request, _jsonSerializerOptions), Encoding.UTF8, "application/json");

    private static async Task<T?> ConvertToContentResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if(content is null)
            return default;

        return JsonSerializer.Deserialize<T>(content, _jsonSerializerOptions);
    }

    private Task<HttpResponseMessage> CreateAuction(object request)
        => _httpClient.PostAsync(AuctionsApi, ConvertToContentRequest(request));

    private Task<HttpResponseMessage> GetAuction(Guid id)
        => _httpClient.GetAsync($"{AuctionsApi}/{id}");

    private Task<HttpResponseMessage> PlaceBid(object request)
        => _httpClient.PostAsync(BidsApi, ConvertToContentRequest(request));

    private Task<HttpResponseMessage> GetBid(Guid id)
        => _httpClient.GetAsync($"{BidsApi}/{id}");
}
