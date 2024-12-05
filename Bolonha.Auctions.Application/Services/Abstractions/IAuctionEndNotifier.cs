namespace Bolonha.Auctions.Application.Services.Abstractions;

public interface IAuctionEndNotifier
{
    Task NotifyAuctionEndedAsync(CancellationToken cancellationToken);
}
