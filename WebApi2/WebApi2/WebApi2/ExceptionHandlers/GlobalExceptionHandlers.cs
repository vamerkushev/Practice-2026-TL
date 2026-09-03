using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebApi2.ExceptionHandlers;

public sealed class GlobalExceptionHandlers : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandlers> _logger;

    public GlobalExceptionHandlers( ILogger<GlobalExceptionHandlers> logger )
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken )
    {
        ProblemDetails problemDetails;

        switch ( exception )
        {
            case NotFoundException notFoundException:
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
                    break;
                }

            case BadRequestException badRequestException:
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
                    break;
                }

            default:
                {
                    _logger.LogError( exception, "Exception occurred: {Message}", exception.Message );
                    problemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "Server error"
                    };
                    break;
                }
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync( problemDetails, cancellationToken );

        return true;
    }
}
