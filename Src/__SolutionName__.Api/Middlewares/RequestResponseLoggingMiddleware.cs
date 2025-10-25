using System.Diagnostics;
using System.Text;

namespace __SolutionName__.Api.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.TraceIdentifier; // Unique ID per request
            var stopwatch = Stopwatch.StartNew();

            // Log request info
            _logger.LogInformation(
                "[{CorrelationId}] Incoming Request: {Method} {Path}{QueryString} from {IP} User: {User}",
                correlationId,
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                context.Connection.RemoteIpAddress?.ToString(),
                context.User?.Identity?.Name ?? "Anonymous"
            );

            // Capture request body (if readable)
            context.Request.EnableBuffering();
            if (context.Request.ContentLength > 0 && context.Request.Body.CanRead)
            {
                var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
                await context.Request.Body.ReadAsync(buffer);
                var bodyAsText = Encoding.UTF8.GetString(buffer);
                _logger.LogDebug("[{CorrelationId}] Request Body: {Body}", correlationId, bodyAsText);

                context.Request.Body.Position = 0; // Reset for next middlewares
            }

            // Capture the response body
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                await _next(context); // Process request pipeline

                stopwatch.Stop();
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation(
                    "[{CorrelationId}] Outgoing Response: {StatusCode} {Elapsed}ms",
                    correlationId,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds
                );

                // Optional: Log the response body for non-binary content
                if (!string.IsNullOrWhiteSpace(responseText))
                {
                    _logger.LogDebug("[{CorrelationId}] Response Body: {Body}", correlationId, responseText);
                }

                // Copy the response back to the original stream
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (TaskCanceledException)
            {
                stopwatch.Stop();
                _logger.LogWarning(
                    "[{CorrelationId}] Request cancelled by client after {Elapsed}ms",
                    correlationId,
                    stopwatch.ElapsedMilliseconds
                );
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "[{CorrelationId}] Exception occurred after {Elapsed}ms", correlationId, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}