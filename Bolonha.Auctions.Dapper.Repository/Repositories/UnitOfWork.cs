using Bolonha.Auctions.Domain.Repositories;

namespace Bolonha.Auctions.Dapper.Repository.Repositories;

public class UnitOfWork(
    IAuctionRepository auctionRepository,
    IAuctionRepository auctionCacheRepository,
    IBidRepository bidRepository,
    IBidRepository bidCacheRepository) 
    : IUnitOfWork
{
    public IAuctionRepository Auctions => auctionRepository;
    public IAuctionRepository CacheAuctions => auctionCacheRepository;
    public IBidRepository Bids => bidRepository;
    public IBidRepository CacheBids => bidCacheRepository;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => Task.FromResult(1);
}
