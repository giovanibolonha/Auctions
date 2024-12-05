using MediatR;
using Microsoft.AspNetCore.Mvc;
using Bolonha.Auctions.Application.Commands;
using Bolonha.Auctions.Application.ViewModels;

namespace Bolonha.Auctions.Api.Controllers;

[Route("api/auctions")]
[ApiController]
public class AuctionsController(IMediator mediator) 
    : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(AuctionViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAuction([FromBody] CreateAuctionCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAuction), new { auctionId = result.Id }, result);
    }


    [HttpGet("{auctionId:guid}")]
    [ProducesResponseType(typeof(AuctionViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAuction(Guid auctionId)
    {
        var result = await _mediator.Send(new GetAuctionByIdCommand(auctionId));
        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
