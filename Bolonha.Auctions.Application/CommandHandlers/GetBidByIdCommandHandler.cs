using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.Mappers;
using Bolonha.Auctions.Application.ViewModels;
using Bolonha.Auctions.Domain.Repositories;
using MediatR;

namespace Bolonha.Auctions.Application.CommandHandlers;

public class GetBidByIdCommandHandler(IUnitOfWork unityOfWork)
    : IRequestHandler<GetBidByIdCommand, BidViewModel?>
{
    private readonly IUnitOfWork _unityOfWork = unityOfWork
       ?? throw new ArgumentNullException(nameof(unityOfWork), "Unit of work must not be null.");

    public async Task<BidViewModel?> Handle(GetBidByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unityOfWork.Bids
            .GetByIdAsync(request.Id, cancellationToken);

        return entity?.ToWiewModel();
    }
}
