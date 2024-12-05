using Bolonha.Auctions.Dapper.Repository.Mappers;
using Bolonha.Auctions.Dapper.Repository.Models;
using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.Repositories;
using Dapper;
using System.Data;

namespace Bolonha.Auctions.Dapper.Repository.Repositories;

public class BidRepository(IDbConnection dbConnection) : IBidRepository
{
    private readonly IDbConnection _dbConnection = dbConnection
        ?? throw new ArgumentNullException(nameof(dbConnection), "DbConnection must not be null.");

    public async Task AddAsync(Bid entity, CancellationToken cancellationToken)
    {
        const string query = @"
            INSERT INTO Bids (Id, Amount, TimeOfBid, AuctionId, Status) 
            VALUES (@BidId, @Amount, @TimeOfBid, @AuctionId, @BidStatus)";

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

    public Task<IEnumerable<Bid>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Bid?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string query = @"
            SELECT Id AS BidId, Amount, TimeOfBid, AuctionId, Status AS BidStatus
            FROM dbo.Bids 
            WHERE Id = @BidId";

        var data = await _dbConnection
            .QueryFirstOrDefaultAsync<BidData>(
                new CommandDefinition(
                    query,
                    new { BidId = id },
                    cancellationToken: cancellationToken));

        return data?.ToDomain();
    }

    public async Task UpdateAsync(Bid entity, CancellationToken cancellationToken)
    {
        const string query = @"
            UPDATE dbo.Bids SET Status = @BidStatus 
            WHERE Id = @BidId";

        await _dbConnection
            .ExecuteAsync(
                new CommandDefinition(
                    query,
                    entity.ToData(),
                    cancellationToken: cancellationToken));
    }
}
