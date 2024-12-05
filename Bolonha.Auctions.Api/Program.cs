using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.Validations;
using Bolonha.Auctions.Domain.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Bolonha.Auctions.Redis.Repository.Repositories;
using MassTransit;
using Bolonha.Auctions.Dapper.Repository.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using Bolonha.Auctions.DataBase.Migrations;
using MediatR;
using Bolonha.Auctions.Application.Behaviors;
using Bolonha.Auctions.Api.Infrastructure.Middlewares;
using Bolonha.Auctions.Api.Infrastructure.BackgroundServices;
using Bolonha.Auctions.Application.Services.Abstractions;
using Bolonha.Auctions.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container

builder.Services.AddDbContextFactory<AuctionDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuctionDB"));
});

builder.Services.AddScoped<IDbConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("AuctionDB");
    return new SqlConnection(connectionString);
});

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("messaging"));
    });
});

builder.AddRedisClient("cache");

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining(typeof(CreateAuctionCommand));
});

builder.Services.AddValidatorsFromAssemblyContaining<CreateAuctionCommandValidator>();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

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

builder.Services.AddScoped<IUnitOfWork>(provider =>
    new UnitOfWork(
        provider.GetRequiredService<IAuctionRepository>(),
        provider.GetRequiredService<AuctionCacheRepository>(),
        provider.GetRequiredService<IBidRepository>(),
        provider.GetRequiredService<BidCacheRepository>()));

builder.Services.AddHostedService<CloseAuctionBackgroundService>();
builder.Services.AddScoped<IAuctionEndNotifier, AuctionEndNotifier>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Services.CreateScope()
    .ServiceProvider.GetRequiredService<AuctionDbContext>().Database.Migrate();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
