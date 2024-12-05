using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.ValueObjects;
using Bolonha.Auctions.Redis.Repository.Models;

namespace Bolonha.Auctions.Redis.Repository.Mappers;

internal static class BidMapper
{
    public static Bid ToDomain(this BidData bidData)
        => new(
            bidData.Id,
            new Money(bidData.Amount),
            bidData.TimeOfBid,
            bidData.AuctionId,
            bidData.Status.ToDomain());

    public static BidData ToData(this Bid entity)
        => new()
        {
            Id = entity.Id,
            Amount = entity.Amount.Amount,
            AuctionId = entity.AuctionId,
            TimeOfBid = entity.TimeOfBid,
            Status = entity.Status.ToStatusCode()
        };

    private static BidStatus ToDomain(this short status)
        => status switch
        {
            0 => BidStatus.Pending,
            1 => BidStatus.Accepted,
            2 => BidStatus.Rejected,
            _ => throw new ArgumentOutOfRangeException($"Invalid status value: {status}")
        };

    private static short ToStatusCode(this BidStatus status)
        => status switch
        {
            BidStatus.Pending => 0,
            BidStatus.Accepted => 1,
            BidStatus.Rejected => 2,
            _ => throw new ArgumentOutOfRangeException($"Invalid BidStatus value: {status}")
        };
}
