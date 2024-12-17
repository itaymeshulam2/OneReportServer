
namespace OneReportServer.Middleware
{
    public static class StartupStartup
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionMiddleware>();
        public static IApplicationBuilder UseGlobalRequestBodyMiddlewareHandler(this IApplicationBuilder app)
            => app.UseMiddleware<RequestBodyMiddleware>();
       
    }
}
