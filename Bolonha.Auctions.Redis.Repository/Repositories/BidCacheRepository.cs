using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.Repositories;
using Bolonha.Auctions.Redis.Repository.Mappers;
using Bolonha.Auctions.Redis.Repository.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Bolonha.Auctions.Redis.Repository.Repositories;

public class BidCacheRepository(IConnectionMultiplexer connection) 
    : IBidRepository
{
    private readonly IDatabase _database = connection.GetDatabase();

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    private static readonly RedisKey KeyPrefix = "bid";

    private static RedisKey GetKey(Guid id) => KeyPrefix.Append(id.ToString());

    public async Task AddAsync(Bid entity, CancellationToken cancellationToken)
    {
        var data = JsonSerializer.Serialize(entity.ToData(), _jsonSerializerOptions);
        await _database.StringSetAsync(GetKey(entity.Id), data, TimeSpan.FromHours(1));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _database.KeyDeleteAsync(GetKey(id));
    }

    public Task<IEnumerable<Bid>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Bid?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var data = await _database.StringGetLeaseAsync(GetKey(id));
        if (data is null || data.Length == 0)
            return null;

        var entity = JsonSerializer.Deserialize<BidData>(data.Span, _jsonSerializerOptions);
        if (entity is null)
            return null;
        
        return entity.ToDomain();
    }

    public async Task UpdateAsync(Bid entity, CancellationToken cancellationToken)
    {
        await AddAsync(entity, cancellationToken);
    }
}
