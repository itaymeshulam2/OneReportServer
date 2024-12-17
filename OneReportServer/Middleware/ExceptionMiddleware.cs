using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
using System.Net;
using OneReportServer.Exceptions;

namespace OneReportServer.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                LogContext.PushProperty("StackTrace", exception.StackTrace);
                var errorResult = new ErrorResult();

                Log.Warning($"Exception! Type: [{exception.GetType()}]. Details: [{JsonConvert.SerializeObject(exception)}]");

                if (exception is not BaseException && exception.InnerException != null)
                {
                    while (exception.InnerException != null)
                    {
                        exception = exception.InnerException;
                    }
                }

                switch (exception)
                {
                    case BaseException e:
                        errorResult.StatusCode = (int)e.StatusCode;
                        errorResult.ErrorTypeText = e.ErrorTypeText;
                        errorResult.ErrorType = e.ErrorType;
                        break;

                    case KeyNotFoundException:
                        errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;

                        
                }
                

                var response = context.Response;
                if (!response.HasStarted)
                {
                    response.ContentType = "application/json";
                    response.StatusCode = errorResult.StatusCode;
                    await response.WriteAsync(JsonConvert.SerializeObject(errorResult));
                }
                else
                {
                    Log.Warning("Can't write error response. Response has already started.");
                }
            }
        }
    }
}
