using Bolonha.Auctions.Dapper.Repository.Models;
using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.ValueObjects;

namespace Bolonha.Auctions.Dapper.Repository.Mappers;

internal static class BidMapper
{
    public static Bid ToDomain(this BidData bidData)
        => new(
            bidData.BidId,
            new Money(bidData.Amount),
            bidData.TimeOfBid,
            bidData.AuctionId,
            bidData.BidStatus.ToDomain());

    public static BidData ToData(this Bid entity)
        => new()
        {
            BidId = entity.Id,
            Amount = entity.Amount.Amount,
            AuctionId = entity.AuctionId,
            TimeOfBid = entity.TimeOfBid,
            BidStatus = entity.Status.ToStatusCode()
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
