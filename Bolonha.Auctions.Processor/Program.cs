using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.EventHandlers;
using Bolonha.Auctions.Application.Validations;
using Bolonha.Auctions.Dapper.Repository.Repositories;
using Bolonha.Auctions.Domain.Repositories;
using Bolonha.Auctions.Redis.Repository.Repositories;
using FluentValidation;
using MassTransit;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("AuctionDB");
    return new SqlConnection(connectionString);
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PlacedBidEventHandler>();
    x.AddConsumer<EndedAuctionEventHandler>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));

        cfg.ReceiveEndpoint("placed-bid-queue", e =>
        {
            e.ConfigureConsumer<PlacedBidEventHandler>(context);
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });

        cfg.ReceiveEndpoint("ended-auction-queue", e =>
        {
            e.ConfigureConsumer<EndedAuctionEventHandler>(context);
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
        });
    });
});

builder.AddRedisClient("cache");

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(CreateAuctionCommand));
});

builder.Services.AddScoped<AuctionRepository>();
builder.Services.AddSingleton<AuctionCacheRepository>();
builder.Services.AddScoped<IAuctionRepository>(provider =>
    new AuctionRepositoryDecorator(
        provider.GetRequiredService<AuctionRepository>(),
        provider.GetRequiredService<AuctionCacheRepository>()));

builder.Services.AddScoped<BidRepository>();
builder.Services.AddSingleton<BidCacheRepository>();
builder.Services.AddScoped<IBidRepository>(provider =>
    new BidRepositoryDecorator(
        provider.GetRequiredService<BidRepository>(),
        provider.GetRequiredService<BidCacheRepository>()));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateAuctionCommandValidator>();

var host = builder.Build();
host.Run();
