using Bolonha.Auctions.Dapper.Repository.Models;
using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.ValueObjects;

namespace Bolonha.Auctions.Dapper.Repository.Mappers;

internal static class AuctionMapper
{
    public static Auction ToDomain(this AuctionData auctionData, BidData? bidData)
        => new(
            auctionData.Id,
            auctionData.Title,
            new Money(auctionData.StartingPrice),
            auctionData.StartTime,
            auctionData.EndTime,
            auctionData.AuctionStatus.ToDomain(),
            bidData?.ToDomain());

    public static AuctionData ToData(this Auction entity)
        => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            EndTime = entity.EndTime,
            StartingPrice = entity.StartingPrice.Amount,
            StartTime = entity.StartTime,
            AuctionStatus = entity.Status.ToStatusCode(),
            WinningBidId = entity.WinningBid?.Id
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
