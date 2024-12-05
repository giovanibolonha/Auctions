using Aspire.Hosting;
using Projects;

namespace Bolonha.Auctions.Tests;

public class AuctionsApiFixture : IAsyncLifetime
{
    private const string ResorceName = "auctions-api";

    private DistributedApplication? _app;

    public async Task DisposeAsync()
    {
        if (_app is not null)
            await _app.DisposeAsync();
    }

    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Bolonha_Auctions_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        _app = await appHost.BuildAsync();
        var resourceNotificationService = _app.Services
            .GetRequiredService<ResourceNotificationService>();
        await _app.StartAsync();
        await resourceNotificationService
            .WaitForResourceAsync(ResorceName, KnownResourceStates.Running)
            .WaitAsync(TimeSpan.FromSeconds(60));
    }

    public HttpClient? CreateHttpClient() 
        => _app?.CreateHttpClient(ResorceName);
}
