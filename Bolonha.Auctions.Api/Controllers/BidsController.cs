using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bolonha.Auctions.Api.Controllers;

[Route("api/bids")]
[ApiController]
public class BidsController(IMediator mediator) 
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(BidViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PlaceBid([FromBody] PlaceBidCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBid), new { bidId = result.Id }, result);
    }

    [HttpGet("{bidId:guid}")]
    [ProducesResponseType(typeof(BidViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBid(Guid bidId)
    {
        var result = await _mediator.Send(new GetBidByIdCommand(bidId));
        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
