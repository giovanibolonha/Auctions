using Bolonha.Auctions.Application.ViewModels;
using MediatR;

namespace Bolonha.Auctions.Application.Commands;

public record PlaceBidCommand(Guid AuctionId, decimal Amount) 
    : IRequest<BidViewModel>;
