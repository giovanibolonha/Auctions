using Bolonha.Auctions.Application.Commands;
using FluentValidation;

namespace Bolonha.Auctions.Application.Validations;

public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        RuleFor(auction => auction.Title)
            .NotEmpty().WithMessage("The auction title is required and cannot be empty.")
            .NotNull().WithMessage("The auction title cannot be null.")
            .MaximumLength(200).WithMessage("The auction title must not exceed 200 characters.");

        RuleFor(auction => auction.StartingPrice)
            .GreaterThanOrEqualTo(0).WithMessage("The starting price must be zero or a positive value.");

        RuleFor(auction => auction.EndTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("The end time must be a future date and time.");
    }
}
