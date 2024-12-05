namespace Bolonha.Auctions.Redis.Repository.Models;

internal class AuctionData
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public decimal StartingPrice { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public short Status { get; init; }
    public BidData? WinningBid { get; init; }
}
