using Bolonha.Auctions.Dapper.Repository.Mappers;
using Bolonha.Auctions.Dapper.Repository.Models;
using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.Repositories;
using Dapper;
using System.Data;
using static Dapper.SqlMapper;

namespace Bolonha.Auctions.Dapper.Repository.Repositories;

public class AuctionRepository(IDbConnection dbConnection)
    : IAuctionRepository
{
    private readonly IDbConnection _dbConnection = dbConnection
        ?? throw new ArgumentNullException(nameof(dbConnection), "DbConnection must not be null.");

    public async Task AddAsync(Auction entity, CancellationToken cancellationToken)
    {
        const string query = @"
            INSERT INTO dbo.Auctions (Id, Title, StartingPrice, StartTime, EndTime, Status)
            VALUES (@Id, @Title, @StartingPrice, @StartTime, @EndTime, @AuctionStatus);";

        await _dbConnection
            .ExecuteAsync(
                new CommandDefinition(
                    query,
                    entity.ToData(),
                    cancellationToken: cancellationToken));
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Auction>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Auction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query = @"
            SELECT 
                a.Id, a.Title, a.StartingPrice, a.StartTime, a.EndTime, a.Status AS AuctionStatus,
                b.Id AS BidId, b.Amount, b.TimeOfBid, b.AuctionId, b.Status AS BidStatus
            FROM dbo.Auctions a
            LEFT JOIN dbo.Bids b ON a.WinningBidId = b.Id
            WHERE a.Id = @Id";

        var entities = await _dbConnection.QueryAsync<AuctionData, BidData, Auction>(
            new CommandDefinition(
                query,
                new { Id = id },
                cancellationToken: cancellationToken),
            (auctionData, bidData) => auctionData.ToDomain(bidData),
            splitOn: "BidId");

        return entities.FirstOrDefault();
    }

    public async Task<IEnumerable<Guid>> GetEndedAuctionIds(short top, CancellationToken cancellationToken)
    {
        const string query = @"
            SELECT TOP (@Top) Id
            FROM dbo.Auctions
            WHERE EndTime <= @EndTime";

        var ids = await _dbConnection.QueryAsync<Guid>(
            new CommandDefinition(
                query,
                new
                {
                    Top = top,
                    EndTime = DateTime.UtcNow,
                },
                cancellationToken: cancellationToken));

        return ids.ToList();
    }

    public async Task UpdateAsync(Auction entity, CancellationToken cancellationToken)
    {
        const string query = @"
            UPDATE dbo.Auctions SET WinningBidId = @WinningBidId, Status = @AuctionStatus 
            WHERE Id = @Id";

        await _dbConnection
            .ExecuteAsync(
                new CommandDefinition(
                    query,
                    entity.ToData(),
                    cancellationToken: cancellationToken));

    }
}