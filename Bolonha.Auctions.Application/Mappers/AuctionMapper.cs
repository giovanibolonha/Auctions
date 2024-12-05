using Bolonha.Auctions.Application.ViewModels;
using Bolonha.Auctions.Domain.Entities;

namespace Bolonha.Auctions.Application.Mappers;

internal static class AuctionMapper
{
    public static AuctionViewModel ToWiewModel(this Auction entity)
        => new()
        {
            Id = entity.Id,
            Title = entity.Title,
            EndTime = entity.EndTime,
            StartingPrice = entity.StartingPrice.Amount,
            StartTime = entity.StartTime,
            Status = entity.Status.ToString(),
            WinningBid = entity.WinningBid?.ToWiewModel(),
        };
}
