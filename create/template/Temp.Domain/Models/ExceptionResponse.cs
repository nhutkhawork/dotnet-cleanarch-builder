namespace Temp.Domain.Models;

public class ExceptionResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
}
