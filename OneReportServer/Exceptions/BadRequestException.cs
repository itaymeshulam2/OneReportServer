using System.Net;

namespace OneReportServer.Exceptions
{
    public class BadRequestException : BaseException
    {

        public BadRequestException(ExceptionErrorType errorType, System.Exception? ex = null, object? data = null)
            : base(HttpStatusCode.BadRequest, errorType, ex, data)
        {
        }
    }
}
