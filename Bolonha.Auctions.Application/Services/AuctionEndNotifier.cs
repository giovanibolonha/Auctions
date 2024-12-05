using Bolonha.Auctions.Application.Services.Abstractions;
using Bolonha.Auctions.Domain.Repositories;
using Bolonha.Auctions.Events.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Bolonha.Auctions.Application.Services;

public class AuctionEndNotifier(
    IUnitOfWork unitOfWork, 
    IPublishEndpoint publishEndpoint,
    ILogger<AuctionEndNotifier> logger) 
    : IAuctionEndNotifier
{
    private readonly IUnitOfWork _unityOfWork = unitOfWork
        ?? throw new ArgumentNullException(nameof(unitOfWork));

    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint
        ?? throw new ArgumentNullException(nameof(publishEndpoint));

    private readonly ILogger<AuctionEndNotifier> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    public async Task NotifyAuctionEndedAsync(CancellationToken cancellationToken)
    {
        try
        {
            var auctions = await _unityOfWork.Auctions
                .GetEndedAuctionIds(100, cancellationToken);

            if (!auctions.Any())
            {
                _logger.LogInformation("No ended auctions found to notify.");
                return;
            }

            var auctionEvents = auctions
                .Select(x => new EndedAuctionEvent(x)).ToList();

            await _publishEndpoint
                .PublishBatch(auctionEvents, cancellationToken);

            _logger.LogInformation("{Count} ended auctions notified successfully.", auctionEvents.Count);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "An error occurred while notifying ended auctions.");
        }
    }
}
