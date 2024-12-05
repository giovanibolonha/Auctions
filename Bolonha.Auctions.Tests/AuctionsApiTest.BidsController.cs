using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.ViewModels;

namespace Bolonha.Auctions.Tests;

public partial class AuctionsApiTest
{
    [Fact]
    public async Task GetBid_ShouldReturn_OK()
    {
        // Arrange
        var auctionRequest = new CreateAuctionCommand(Guid.NewGuid().ToString(), 0, DateTime.UtcNow.AddDays(1));
        var auctionResponseApi = await CreateAuction(auctionRequest);
        var auctionResponse = await ConvertToContentResponse<AuctionViewModel>(auctionResponseApi);

        Assert.NotNull(auctionResponse);

        var bidRequest = new PlaceBidCommand(auctionResponse.Id, 10);
        var bidResponseApi = await PlaceBid(bidRequest);
        var bidResponse = await ConvertToContentResponse<BidViewModel>(bidResponseApi);

        Assert.NotNull(bidResponse);

        // Act
        var actualResponseApi = await GetBid(bidResponse.Id);

        // Assert
        Assert.Equal(HttpStatusCode.OK, actualResponseApi.StatusCode);

        var actualResponse = await ConvertToContentResponse<BidViewModel>(actualResponseApi);

        Assert.NotNull(actualResponse);
    }

    [Fact]
    public async Task GetBid_ShouldReturn_NotFound()
    {
        // Arrange
        var id = new Guid("71198E23-A5C0-45FC-B9B2-11A253D5E1E9");

        // Act
        var actualResponseApi = await GetBid(id);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actualResponseApi.StatusCode);
    }

    [Fact]
    public async Task PlaceBid_ShouldReturn_Created()
    {
        // Arrange
        var auctionRequest = new CreateAuctionCommand(Guid.NewGuid().ToString(), 0, DateTime.UtcNow.AddDays(1));
        var auctionResponseApi = await CreateAuction(auctionRequest);
        var auctionResponse = await ConvertToContentResponse<AuctionViewModel>(auctionResponseApi);

        Assert.NotNull(auctionResponse);

        var request = new PlaceBidCommand(auctionResponse.Id, 10);
        var actualResponseApi = await PlaceBid(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, actualResponseApi.StatusCode);

        var actualResponse = await ConvertToContentResponse<BidViewModel>(actualResponseApi);

        Assert.NotNull(actualResponse);
    }

    [Theory]
    [MemberData(nameof(PlaceBid_ShouldReturn_BadRequestData))]
    public async Task PlaceBid_ShouldReturn_BadRequest(Guid auctionId, decimal amount)
    {
        // Arrange
        var request = new PlaceBidCommand(auctionId, amount);

        // Act
        var actualResponseApi = await PlaceBid(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actualResponseApi.StatusCode);
    }

    public static IEnumerable<object[]> PlaceBid_ShouldReturn_BadRequestData() =>
        [
            [Guid.NewGuid(), -1],
            [Guid.NewGuid(), 0],
            [Guid.Empty, 100],
        ];

    [Fact]
    public async Task PlaceBid_ShouldReturn_WinningBid()
    {
        // Arrange
        var auctionRequest = new CreateAuctionCommand(Guid.NewGuid().ToString(), 0, DateTime.UtcNow.AddDays(1));
        var auctionResponseApi = await CreateAuction(auctionRequest);
        var auctionResponse = await ConvertToContentResponse<AuctionViewModel>(auctionResponseApi);
        Assert.NotNull(auctionResponse);

        // Act
        var bidResponseApi = await PlaceBid(new PlaceBidCommand(auctionResponse.Id, 10));
        Assert.Equal(HttpStatusCode.Created, bidResponseApi.StatusCode);

        await Task.Delay(TimeSpan.FromSeconds(10));

        var responseApi = await GetAuction(auctionResponse.Id);

        // Assert
        Assert.Equal(HttpStatusCode.OK, responseApi.StatusCode);

        var response = await ConvertToContentResponse<AuctionViewModel>(responseApi);

        Assert.NotNull(response);
        Assert.Equal(10, response.WinningBid?.Amount);
    }
}
