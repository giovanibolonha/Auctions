using Bolonha.Auctions.Application.ViewModels;
using MediatR;

namespace Bolonha.Auctions.Application.Commands;

public record GetAuctionByIdCommand(Guid Id) 
    : IRequest<AuctionViewModel>;
