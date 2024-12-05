using Bolonha.Auctions.Domain.Entities;
using Bolonha.Auctions.Domain.Repositories;
using System.Threading;

namespace Bolonha.Auctions.Redis.Repository.Repositories;

public class AuctionRepositoryDecorator(
    IAuctionRepository auctionRepository, 
    IAuctionRepository auctionCacheRepository) 
    : IAuctionRepository
{
    private readonly IAuctionRepository _auctionRepository = auctionRepository;
    private readonly IAuctionRepository _auctionCacheRepository = auctionCacheRepository;

    public async Task AddAsync(Auction entity, CancellationToken cancellationToken)
    {
        await _auctionRepository.AddAsync(entity, cancellationToken);
        await _auctionCacheRepository.AddAsync(entity, cancellationToken);
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
        var entity = await _auctionCacheRepository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
            return entity;

        entity = await _auctionRepository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
            await _auctionCacheRepository.AddAsync(entity, cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<Guid>> GetEndedAuctionIds(short top, CancellationToken cancellationToken)
    {
        return await _auctionRepository.GetEndedAuctionIds(top, cancellationToken);
    }

    public async Task UpdateAsync(Auction entity, CancellationToken cancellationToken)
    {
        await _auctionRepository.UpdateAsync(entity, cancellationToken);
        await _auctionCacheRepository.UpdateAsync(entity, cancellationToken);
    }
}
