namespace Bolonha.Auctions.Redis.Repository.Models;

internal class BidData
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public DateTime TimeOfBid { get; init; }
    public Guid AuctionId { get; init; }
    public short Status { get; init; }
}
