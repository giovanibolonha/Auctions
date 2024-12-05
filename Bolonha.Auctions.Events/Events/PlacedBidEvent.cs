namespace Bolonha.Auctions.Events.Events;

public record PlacedBidEvent(
    Guid BidId,
    Guid AuctionId, 
    decimal Amount, 
    DateTime TimeOfBid,
    short Status);
