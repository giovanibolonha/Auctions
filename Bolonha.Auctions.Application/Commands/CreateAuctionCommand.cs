using Bolonha.Auctions.Application.ViewModels;
using MediatR;

namespace Bolonha.Auctions.Application.Commands;

public record CreateAuctionCommand(
    string Title, 
    decimal StartingPrice,  
    DateTime EndTime) 
    : IRequest<AuctionViewModel>;
