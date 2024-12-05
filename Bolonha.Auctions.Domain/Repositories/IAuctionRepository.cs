using Bolonha.Auctions.Domain.Entities;

namespace Bolonha.Auctions.Domain.Repositories;

public interface IAuctionRepository : IRepository<Auction>
{
    Task<IEnumerable<Guid>> GetEndedAuctionIds(short top, CancellationToken cancellationToken);
}