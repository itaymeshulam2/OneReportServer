using System.Net;

namespace OneReportServer.Exceptions;

public class UnauthorizedException : BaseException
{
    public UnauthorizedException(ExceptionErrorType errorType, System.Exception? ex = null, object? data = null)
        : base(HttpStatusCode.Unauthorized, errorType, ex, data)
    {
    }
}