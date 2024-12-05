using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Bolonha.Auctions.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            var commandName = typeof(TRequest).Name;
            _logger.LogDebug("Handling command {CommandName} with request data: {@Request}", commandName, request);
            var response = await next();
            _logger.LogDebug("Successfully handled command {CommandName} with response: {@Response}", commandName, response);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Successfully handled command {CommandName}", typeof(TRequest).Name);
            throw;
        }
    }
}