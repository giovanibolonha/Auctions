namespace Bolonha.Auctions.Dapper.Repository.Models;

internal class BidData
{
    public Guid BidId { get; init; }
    public decimal Amount { get; init; }
    public DateTime TimeOfBid { get; init; }
    public Guid AuctionId { get; init; }
    public short BidStatus { get; init; }
}
