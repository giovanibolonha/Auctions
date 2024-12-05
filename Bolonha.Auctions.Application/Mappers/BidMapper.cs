using Bolonha.Auctions.Application.ViewModels;
using Bolonha.Auctions.Domain.Entities;

namespace Bolonha.Auctions.Application.Mappers;

internal static class BidMapper
{
    public static BidViewModel ToWiewModel(this Bid entity)
        => new()
        {
            Id = entity.Id,
            Amount = entity.Amount.Amount,
            TimeOfBid = entity.TimeOfBid,
            AuctionId = entity.AuctionId,
            Status = entity.Status.ToString(),
        };
}
