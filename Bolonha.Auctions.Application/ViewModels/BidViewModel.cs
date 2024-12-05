namespace Bolonha.Auctions.Application.ViewModels;

public class BidViewModel
{
    public Guid Id { get; init; }
    public decimal Amount { get; init; }
    public DateTime TimeOfBid { get; init; }
    public Guid AuctionId { get; init; }
    public required string Status { get; init; }
}
