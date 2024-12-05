using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.ViewModels;

namespace Bolonha.Auctions.Tests;

public partial class AuctionsApiTest
{
    [Fact]
    public async Task GetAuction_ShouldReturn_OK()
    {
        // Arrange
        var auctionRequest = new CreateAuctionCommand(Guid.NewGuid().ToString(), 0, DateTime.UtcNow.AddDays(1));
        var auctionResponseApi = await CreateAuction(auctionRequest);
        var auctionResponse = await ConvertToContentResponse<AuctionViewModel>(auctionResponseApi);

        Assert.NotNull(auctionResponse);

        // Act
        var actualResponseApi = await GetAuction(auctionResponse.Id);

        // Assert
        Assert.Equal(HttpStatusCode.OK, actualResponseApi.StatusCode);

        var actualResponse = await ConvertToContentResponse<AuctionViewModel>(actualResponseApi);

        Assert.Equivalent(auctionResponse, actualResponse);
        Assert.NotNull(actualResponse);
    }

    [Fact]
    public async Task GetAuction_ShouldReturn_NotFound()
    {
        // Arrange
        var id = new Guid("71198E23-A5C0-45FC-B9B2-11A253D5E1E9");

        // Act
        var actualResponseApi = await GetAuction(id);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, actualResponseApi.StatusCode);
    }

    [Fact]
    public async Task CreateAuction_ShouldReturn_Created()
    {
        // Arrange
        var request = new CreateAuctionCommand(Guid.NewGuid().ToString(), 100, DateTime.UtcNow.AddDays(1));

        // Act
        var actualResponseApi = await CreateAuction(request);

        // Assert
        Assert.Equal(HttpStatusCode.Created, actualResponseApi.StatusCode);

        var actualResponse = await ConvertToContentResponse<AuctionViewModel>(actualResponseApi);
        Assert.NotNull(actualResponse);
    }

    [Theory]
    [MemberData(nameof(CreateAuction_ShouldReturn_BadRequestData))]
    public async Task CreateAuction_ShouldReturn_BadRequest(string title, decimal startingPrice, DateTime endTime)
    {
        // Arrange
        var request = new CreateAuctionCommand(title, startingPrice, endTime);

        // Act
        var actualResponseApi = await CreateAuction(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, actualResponseApi.StatusCode);
    }

    public static IEnumerable<object[]> CreateAuction_ShouldReturn_BadRequestData() =>
        [
            [Guid.NewGuid().ToString() ,- 1, DateTime.UtcNow.AddDays(1)],
            [Guid.NewGuid().ToString(), 0, DateTime.UtcNow.AddDays(-1)],
            [string.Empty, 10, DateTime.UtcNow.AddDays(-1)]
        ];
}
