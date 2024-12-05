using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.Mappers;
using Bolonha.Auctions.Application.ViewModels;
using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.Repositories;
using Bolonha.Auctions.Domain.ValueObjects;
using Bolonha.Auctions.Events.Events;
using MassTransit;
using MediatR;

namespace Bolonha.Auctions.Application.CommandHandlers;

public class PlaceBidCommandHandler(
    IUnitOfWork unityOfWork,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<PlaceBidCommand, BidViewModel>
{
    private readonly IUnitOfWork _unityOfWork = unityOfWork
       ?? throw new ArgumentNullException(nameof(unityOfWork));

    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint
        ?? throw new ArgumentNullException(nameof(publishEndpoint));

    public async Task<BidViewModel> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        var bid = new Bid(
            new Money(request.Amount),
            request.AuctionId);

        await _unityOfWork.CacheBids
            .AddAsync(bid, cancellationToken);

        await _publishEndpoint
            .Publish(new PlacedBidEvent(
                bid.Id,
                bid.AuctionId,
                bid.Amount.Amount,
                bid.TimeOfBid,
                (short)bid.Status), cancellationToken);

        return bid.ToWiewModel();
    }
}
