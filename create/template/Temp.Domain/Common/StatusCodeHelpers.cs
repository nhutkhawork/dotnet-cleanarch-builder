using Temp.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Temp.Domain.Common;

public static class StatusCodeHelpers
{
    public static int ExceptionToStatusCode(Exception? exception) => exception switch
    {
        BadRequestException => StatusCodes.Status400BadRequest,
        UnauthorizedException => StatusCodes.Status401Unauthorized,
        NotFoundException => StatusCodes.Status404NotFound,
        ConflictException => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
}
