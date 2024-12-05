namespace Bolonha.Auctions.DataBase.Migrations.Models;

public class BidData
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public DateTime TimeOfBid { get; set; }

    public AuctionData? Auction { get; set; }

    public Guid AuctionId { get; set; }

    public short Status { get; set; }
}
