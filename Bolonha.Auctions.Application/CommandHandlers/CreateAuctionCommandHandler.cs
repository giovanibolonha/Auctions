using MediatR;
using Bolonha.Auctions.Domain.Repositories;
using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Application.ViewModels;
using Bolonha.Auctions.Domain.ValueObjects;
using Bolonha.Auctions.Application.Mappers;

namespace Bolonha.Auctions.Application.CommandHandlers;

public class CreateAuctionCommandHandler(IUnitOfWork unityOfWork) 
    : IRequestHandler<CreateAuctionCommand, AuctionViewModel>
{
    private readonly IUnitOfWork _unityOfWork = unityOfWork
        ?? throw new ArgumentNullException(nameof(unityOfWork));

    public async Task<AuctionViewModel> Handle(CreateAuctionCommand request, CancellationToken cancellationToken)
    {
        var entity = new Auction(
            Guid.NewGuid(),
            request.Title,
            new Money(request.StartingPrice),
            DateTime.UtcNow,
            request.EndTime);

        await _unityOfWork.Auctions
            .AddAsync(entity, cancellationToken);

        return entity.ToWiewModel();
    }
}
