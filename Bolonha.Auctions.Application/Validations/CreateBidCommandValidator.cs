using Bolonha.Auctions.Application.Commands;
using FluentValidation;

namespace Bolonha.Auctions.Application.Validations;

public class CreateAuctionBidValidator : AbstractValidator<PlaceBidCommand>
{
    public CreateAuctionBidValidator()
    {
        RuleFor(auction => auction.AuctionId).NotEqual(Guid.Empty).WithMessage("Auction ID must be a valid non-empty GUID.");
        RuleFor(auction => auction.Amount).GreaterThan(0).WithMessage("The bid amount must be greater than zero.");
    }
}
