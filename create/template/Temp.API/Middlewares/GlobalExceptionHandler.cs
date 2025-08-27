using Temp.Domain.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Temp.API.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception? exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, exception?.Message);

        var statusCode = StatusCodeHelpers.ExceptionToStatusCode(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = exception?.GetType().Name,
            Detail = exception?.InnerException?.Message ?? exception?.Message,
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
