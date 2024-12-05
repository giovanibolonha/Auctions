using Bolonha.Auctions.Application.ViewModels;
using MediatR;

namespace Bolonha.Auctions.Application.Commands;

public record GetBidByIdCommand(Guid Id) 
    : IRequest<BidViewModel?>;
