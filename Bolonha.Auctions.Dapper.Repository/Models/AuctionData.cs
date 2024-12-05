namespace Bolonha.Auctions.Dapper.Repository.Models;

internal class AuctionData
{
    public Guid Id { get; init; }
    public required string Title { get; init; }
    public decimal StartingPrice { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public short AuctionStatus { get; init; }
    public Guid? WinningBidId { get; init; }
}
