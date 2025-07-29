using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString();

            using (LogContext.PushProperty("RequestId", requestId))
            using (LogContext.PushProperty("RequestPath", context.Request.Path))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            {
                try
                {
                    _logger.LogInformation("Request started: {Method} {Path}", context.Request.Method, context.Request.Path);

                    await _next(context);

                    stopwatch.Stop();
                    var duration = stopwatch.ElapsedMilliseconds;

                    if (duration > 1000)
                    {
                        _logger.LogWarning("Slow request detected: {Method} {Path} took {Duration}ms", 
                            context.Request.Method, context.Request.Path, duration);
                    }
                    else
                    {
                        _logger.LogInformation("Request completed: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms", 
                            context.Request.Method, context.Request.Path, context.Response.StatusCode, duration);
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    var duration = stopwatch.ElapsedMilliseconds;

                    _logger.LogError(ex, "Request failed: {Method} {Path} - Duration: {Duration}ms", 
                        context.Request.Method, context.Request.Path, duration);
                    throw;
                }
            }
        }
    }
} 