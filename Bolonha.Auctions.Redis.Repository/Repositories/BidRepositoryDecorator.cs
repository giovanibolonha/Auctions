using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.Repositories;

namespace Bolonha.Auctions.Redis.Repository.Repositories;

public class BidRepositoryDecorator(
    IBidRepository auctionRepository,
    IBidRepository auctionCacheRepository) 
    : IBidRepository
{
    private readonly IBidRepository _auctionRepository = auctionRepository;
    private readonly IBidRepository _auctionCacheRepository = auctionCacheRepository;

    public async Task AddAsync(Bid entity, CancellationToken cancellationToken)
    {
        await _auctionRepository.AddAsync(entity, cancellationToken);
        await _auctionCacheRepository.AddAsync(entity, cancellationToken);
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
        var entity = await _auctionCacheRepository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
            return entity;

        entity = await _auctionRepository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
            await _auctionCacheRepository.AddAsync(entity, cancellationToken);

        return entity;
    }

    public async Task UpdateAsync(Bid entity, CancellationToken cancellationToken)
    {
        await _auctionRepository.UpdateAsync(entity, cancellationToken);
        await _auctionCacheRepository.UpdateAsync(entity, cancellationToken);
    }
}
