using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi2.ExceptionHandler;

public sealed class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<NotFoundException> _logger;

    public NotFoundExceptionHandler( ILogger<NotFoundException> logger )
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken )
    {
        if ( exception is not NotFoundException notFoundException )
        {
            return false;
        }

        _logger.LogError(
            notFoundException,
            "Exception occurred: {Message}",
            notFoundException.Message );

        ProblemDetails problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Not Found",
            Detail = notFoundException.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync( problemDetails, cancellationToken );

        return true;
    }
}
