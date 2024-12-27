// Middleware/RequestLoggingMiddleware.cs
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace usersAuthApi.Middleware
{
    public class requestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<requestLoggingMiddleware> _logger;

        public requestLoggingMiddleware(RequestDelegate next, ILogger<requestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request details
            var watch = Stopwatch.StartNew();
            _logger.LogInformation("Incoming request: {method} {url}", context.Request.Method, context.Request.Path);

            // Pass the context to the next middleware in the pipeline
            await _next(context);

            // Log response details
            watch.Stop();
            _logger.LogInformation("Response: {method} {url} - Status Code: {statusCode} - Time taken: {time}ms",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, watch.ElapsedMilliseconds);
        }
    }
}
