using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.Repositories;
using Bolonha.Auctions.Domain.ValueObjects;
using Bolonha.Auctions.Events.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Bolonha.Auctions.Application.EventHandlers;

public class PlacedBidEventHandler(
    IUnitOfWork unityOfWork,
    ILogger<PlacedBidEventHandler> logger)
    : IConsumer<PlacedBidEvent>
{
    private readonly IUnitOfWork _unityOfWork = unityOfWork
        ?? throw new ArgumentNullException(nameof(unityOfWork));

    protected readonly ILogger _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    public async Task Consume(ConsumeContext<PlacedBidEvent> context)
    {
        var message = context.Message;
        var auction = await _unityOfWork.Auctions
            .GetByIdAsync(message.AuctionId, context.CancellationToken);

        if (auction is null)
        {
            _logger.LogWarning("Auction with Id={AuctionId} could not be found while processing bid with Id={BidId}.",
                message.AuctionId, message.BidId);
            return;
        }
        var bid = new Bid(
            message.BidId,
            new Money(message.Amount),
            message.TimeOfBid,
            message.AuctionId,
            (BidStatus)message.Status);

        auction.PlaceBid(bid);

        await _unityOfWork.Bids
            .AddAsync(bid, context.CancellationToken);

        await _unityOfWork.Auctions
            .UpdateAsync(auction, context.CancellationToken);
    }
}
