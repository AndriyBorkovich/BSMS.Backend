using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

namespace BSMS.Application.Helpers;

public class MethodResult<T>
{
    private readonly ILogger _logger;

    public MethodResult(ILogger logger)
    {
        _logger = logger;
    }

    public ErrorResponse Error { get; set; }
    public HttpStatusCode ErrorStatusCode { get; set; }
    public T Data { get; set; }
    public bool IsError => Error is { };

    public void SetError(string errorMessage, HttpStatusCode statusCode,  object errorData = null)
    {
        Error = new ErrorResponse
        {
            ErrorMessage = errorMessage,
            ErrorData = errorData
        };

        ErrorStatusCode = statusCode;

        _logger.LogWarning("{message}", Error.ErrorMessage);
    }

    public void SetError(Exception exception)
    {
        InitializeError(exception.Message, HttpStatusCode.ServiceUnavailable, exception);
    }
    

    private void InitializeError(string errorMessage, HttpStatusCode statusCode, Exception error)
    {
        var message = new StringBuilder(errorMessage);
        while (error != null)
        {
            message.Append(" & ");
            message.Append(error.Message);
            error = error.InnerException;
        }

        errorMessage = message.ToString();
        ErrorStatusCode = statusCode;
        Error = new ErrorResponse
        {
            ErrorMessage = errorMessage
        };

        _logger.LogWarning(error, "{message}", Error.ErrorMessage);
    }
}

public class ErrorResponse
{
    public string ErrorMessage { get; set; }
    public object ErrorData { get; set; }
}
