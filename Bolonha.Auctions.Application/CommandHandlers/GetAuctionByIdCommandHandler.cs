using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.Mappers;
using Bolonha.Auctions.Application.ViewModels;
using Bolonha.Auctions.Domain.Repositories;
using MediatR;

namespace Bolonha.Auctions.Application.CommandHandlers;

public class GetAuctionByIdCommandHandler(IUnitOfWork unityOfWork) 
    : IRequestHandler<GetAuctionByIdCommand, AuctionViewModel?>
{
    private readonly IUnitOfWork _unityOfWork = unityOfWork
       ?? throw new ArgumentNullException(nameof(unityOfWork));

    public async Task<AuctionViewModel?> Handle(GetAuctionByIdCommand request, CancellationToken cancellationToken)
    {
        var entity = await _unityOfWork.Auctions
            .GetByIdAsync(request.Id, cancellationToken);

        return entity?.ToWiewModel();
    }
}
