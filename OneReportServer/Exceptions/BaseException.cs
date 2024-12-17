using System.Net;

namespace OneReportServer.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode;
        public int ErrorType { get; set; }
        public string ErrorTypeText { get; set; }
        public string? Message;
        public object? Data;
        public System.Exception? Ex;
        public BaseException(HttpStatusCode statusCode, ExceptionErrorType errorType, System.Exception? ex, object? data) 
        {
            StatusCode = statusCode;
            ErrorType = (int)errorType;
            ErrorTypeText = errorType.ToString();
            Data = data;
            Ex = ex;
        }
    }
}
