using Bolonha.Auctions.Domain.ValueObjects;

namespace Bolonha.Auctions.Domain.Entities;

public class Bid(
    Guid id,
    Money amount,
    DateTime timeOfBid,
    Guid auctionId,
    BidStatus status = BidStatus.Pending)
{
    public Guid Id => id;
    public Money Amount => amount.Amount > 0 ? amount : throw new ArgumentException();
    public DateTime TimeOfBid => timeOfBid;
    public Guid AuctionId => auctionId;
    public BidStatus Status { get; private set; } = status;

    public Bid(Money amount, Guid auctionId)
        : this(Guid.NewGuid(), amount, DateTime.UtcNow, auctionId)
    {
    }

    public void SetRejectedStatus()
    {
        if (Status == BidStatus.Pending)
            Status = BidStatus.Rejected;
    }

    public void SetAcceptedStatus()
    {
        if (Status == BidStatus.Pending)
            Status = BidStatus.Accepted;
    }

    public bool IsOutBidBy(Bid other)
        => other.Amount.IsGreater(Amount) || (other.Equals(Amount) && TimeOfBid > other.TimeOfBid);
}
