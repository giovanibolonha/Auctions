using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.ValueObjects;
using Bolonha.Auctions.Redis.Repository.Models;

namespace Bolonha.Auctions.Redis.Repository.Mappers;

internal static class AuctionMapper
{
    public static Auction ToDomain(this AuctionData data)
        => new(
            data.Id,
            data.Title,
            new Money(data.StartingPrice),
            data.StartTime,
            data.EndTime,
            data.Status.ToDomain(),
            data.WinningBid?.ToDomain());

    public static AuctionData ToData(this Auction entity)
        => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            EndTime = entity.EndTime,
            StartingPrice = entity.StartingPrice.Amount,
            StartTime = entity.StartTime,
            Status = entity.Status.ToStatusCode(),
            WinningBid = entity.WinningBid?.ToData()
        };

    private static AuctionStatus ToDomain(this short status)
        => status switch
        {
            1 => AuctionStatus.Active,
            2 => AuctionStatus.Closed,
            _ => throw new ArgumentOutOfRangeException($"Invalid status value: {status}")
        };

    private static short ToStatusCode(this AuctionStatus status)
        => status switch
        {
            AuctionStatus.Active => 1,
            AuctionStatus.Closed => 2,
            _ => throw new ArgumentOutOfRangeException($"Invalid AuctionStatus value: {status}")
        };
}
