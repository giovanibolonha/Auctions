using Bolonha.Auctions.Application.Services.Abstractions;

namespace Bolonha.Auctions.Api.Infrastructure.BackgroundServices;

public class CloseAuctionBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<CloseAuctionBackgroundService> logger) 
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{ServiceName} is starting at {StartTime}.", nameof(CloseAuctionBackgroundService), DateTime.UtcNow);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var auctionEndNotifier = scope.ServiceProvider.GetRequiredService<IAuctionEndNotifier>();
                await auctionEndNotifier.NotifyAuctionEndedAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (Exception ex) 
            {
                logger.LogError(ex, "An error occurred while starting {ServiceName} at {StartTime}.", nameof(CloseAuctionBackgroundService), DateTime.UtcNow);
            }
        }

        logger.LogInformation("{ServiceName} is ending at {StartTime}.", nameof(CloseAuctionBackgroundService), DateTime.UtcNow);
    }
}
