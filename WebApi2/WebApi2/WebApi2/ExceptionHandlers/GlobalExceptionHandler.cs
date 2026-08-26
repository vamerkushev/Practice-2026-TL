using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi2.ExceptionHandler;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler( ILogger<GlobalExceptionHandler> logger )
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken )
    {
        ProblemDetails problemDetails;

        if ( exception is NotFoundException notFoundException )
        {
            _logger.LogError(
                notFoundException,
                "Exception occurred: {Message}",
                notFoundException.Message );

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not Found",
                Detail = notFoundException.Message
            };
        }
        else if ( exception is BadRequestException badRequestException )
        {
            _logger.LogError(
                badRequestException,
                "Exception occurred: {Message}",
                badRequestException.Message );

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Bad Request",
                Detail = badRequestException.Message
            };
        }
        else
        {
            _logger.LogError( exception, "Exception occurred: {Message}", exception.Message );

            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            };
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync( problemDetails, cancellationToken );

        return true;
    }
}
