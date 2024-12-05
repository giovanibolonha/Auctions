var builder = DistributedApplication.CreateBuilder(args);

var sqlPassword = builder.AddParameter("sql-password");
var sqlServer = builder.AddSqlServer("sql-server", sqlPassword, port: 1433)
    .WithDataVolume();

var auctionDb = sqlServer
    .AddDatabase("AuctionDB");

var cache = builder
    .AddRedis("cache");

var rabbitMq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin();

var auctionsApi = builder.AddProject<Projects.Bolonha_Auctions_Api>("auctions-api")
    .WithHttpsEndpoint(port: 7095, targetPort: 7095, "K6", isProxied: false)
    .WithReference(auctionDb).WaitFor(auctionDb)
    .WithReference(cache).WaitFor(cache)
    .WithReference(rabbitMq).WaitFor(rabbitMq);

var auctionsProcessor = builder.AddProject<Projects.Bolonha_Auctions_Processor>("auctions-processor")
    .WithReference(auctionDb).WaitFor(auctionDb)
    .WithReference(cache).WaitFor(cache)
    .WithReference(rabbitMq).WaitFor(rabbitMq)
    .WaitFor(auctionsApi);

builder.Build().Run();
