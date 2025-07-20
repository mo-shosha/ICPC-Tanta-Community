using Microsoft.Extensions.Caching.Memory;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using Core.DTO;
using System.Security.Claims;
namespace ICPC_Tanta_Web.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
        //private const int _requestLimit = 8;

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurity(context);

                if (HttpMethods.IsPost(context.Request.Method) ||
                    HttpMethods.IsPut(context.Request.Method) ||
                    HttpMethods.IsPatch(context.Request.Method))
                {
                    if (!IsRequestAllowed(context))
                    {
                        context.Response.Clear();
                        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                        context.Response.ContentType = "application/json";

                        var response = ApiResponse<string>.ErrorResponse("Too many requests, please try again later");
                        var jsonResponse = JsonSerializer.Serialize(response);

                        await context.Response.WriteAsync(jsonResponse);
                        return;
                    }
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = ApiResponse<string>.ErrorResponse(ex.Message);
                var jsonResponse = JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(jsonResponse);
            }
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            string identifier;

            if (context.User.Identity?.IsAuthenticated == true)
            {
                identifier = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown-user";
            }
            else
            {
                identifier = context.Connection.RemoteIpAddress?.ToString() ?? "unknown-ip";
            }

            var cacheKey = $"Rate:{identifier}";
            var now = DateTime.UtcNow;

            var cacheEntry = _memoryCache.GetOrCreate(cacheKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (Timestamp: now, Count: 1);
            });

            var (timestamp, count) = cacheEntry;

            int limit = context.User.Identity?.IsAuthenticated == true ? 30 : 10; //  30 للمسجل، 10 للزائر

            if (now - timestamp < _rateLimitWindow)
            {
                if (count >= limit)
                    return false;

                _memoryCache.Set(cacheKey, (timestamp, count + 1), _rateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cacheKey, (now, 1), _rateLimitWindow);
            }

            return true;
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";

            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";

            context.Response.Headers["X-Frame-Options"] = "DENY";
            
            context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self'";

            context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";

            context.Response.Headers["Referrer-Policy"] = "same-origin";

            context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";

            context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin";

            context.Response.Headers["Cross-Origin-Resource-Policy"] = "same-origin";

            context.Response.Headers["Cross-Origin-Embedder-Policy"] = "require-corp";
            
        }
    }
}
