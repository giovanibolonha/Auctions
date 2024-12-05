namespace Bolonha.Auctions.Domain.Repositories;

public interface IUnitOfWork
{
    IAuctionRepository Auctions { get; }
    IAuctionRepository CacheAuctions { get; }
    IBidRepository CacheBids { get; }
    IBidRepository Bids { get; }
}