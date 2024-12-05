namespace Bolonha.Auctions.DataBase.Migrations.Models;

public class AuctionData
{
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public decimal StartingPrice { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public short Status { get; set; }

    public BidData? WinningBid { get; set; }

    public Guid? WinningBidId { get; set; }

    public List<BidData> Bids { get; set; } = [];
}
