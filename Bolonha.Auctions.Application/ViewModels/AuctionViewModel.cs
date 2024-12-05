namespace Bolonha.Auctions.Application.ViewModels;

public class AuctionViewModel
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public decimal StartingPrice { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public required string Status { get; init; }
    public BidViewModel? WinningBid { get; init; }
}