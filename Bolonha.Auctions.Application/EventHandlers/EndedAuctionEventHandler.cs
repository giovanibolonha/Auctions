using Bolonha.Auctions.Domain.Repositories;
using Bolonha.Auctions.Events.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Bolonha.Auctions.Application.EventHandlers;

public class EndedAuctionEventHandler(
    IUnitOfWork unityOfWork,
    ILogger<EndedAuctionEventHandler> logger)
    : IConsumer<EndedAuctionEvent>
{
    private readonly IUnitOfWork _unityOfWork = unityOfWork
        ?? throw new ArgumentNullException(nameof(unityOfWork));

    protected readonly ILogger _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    public async Task Consume(ConsumeContext<EndedAuctionEvent> context)
    {
        var message = context.Message;
        var auction = await _unityOfWork.Auctions
            .GetByIdAsync(message.AuctionId, context.CancellationToken);

        if (auction is null)
        {
            _logger.LogWarning("Auction with Id={AuctionId} could not be found while closing.", message.AuctionId);
            return;
        }

        auction.Close();

        await _unityOfWork.Auctions
            .UpdateAsync(auction, context.CancellationToken);
    }
}
