using Bolonha.Auctions.Domain.ValueObjects;

namespace Bolonha.Auctions.Domain.Entities;

public class Auction(
    Guid id,
    string title,
    Money startingPrice,
    DateTime startTime,
    DateTime endTime,
    AuctionStatus status = AuctionStatus.Active,
    Bid? winningBid = null)
{
    public Guid Id => id;
    public string Title => title;
    public Money StartingPrice => startingPrice;
    public DateTime StartTime => startTime;
    public DateTime EndTime => endTime;
    public AuctionStatus Status { get; private set; } = status;
    public Bid? WinningBid { get; private set; } = winningBid;

    public void PlaceBid(Bid bid)
    {
        if (!IsBidWithinAuctionTime(bid))
        {
            bid.SetRejectedStatus();
            return;
        }

        bid.SetAcceptedStatus();

        if (!IsBidAboveOrEqualToStartingPrice(bid))
            return;

        if (IsOutBidBy(bid))
            WinningBid = bid;
    }

    public void Close()
    {
        if (Status == AuctionStatus.Active)
            Status = AuctionStatus.Closed;
    }

    private bool IsBidWithinAuctionTime(Bid bid)
        => bid.TimeOfBid >= StartTime && EndTime >= bid.TimeOfBid;

    private bool IsBidAboveOrEqualToStartingPrice(Bid bid)
        => bid.Amount.IsGreater(StartingPrice) || bid.Amount.Equals(StartingPrice);

    private bool IsOutBidBy(Bid bid)
        => WinningBid is null || WinningBid.IsOutBidBy(bid);
}
