using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;
using Core.DTO;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace ICPC_Tanta_Web.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);
        private const int GuestLimit = 10;
        private const int AuthenticatedLimit = 30;

        public ExceptionMiddleware(RequestDelegate next, IMemoryCache memoryCache)
        {
            _next = next;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurityHeaders(context);
                if (HttpMethods.IsPost(context.Request.Method) ||
                    HttpMethods.IsPut(context.Request.Method) ||
                    HttpMethods.IsPatch(context.Request.Method))
                {
                    if (!IsRequestAllowed(context))
                    {
                        await RejectRequest(context, 429, "Too many requests, please try again later");
                        return;
                    }

                    
                    if (context.Request.ContentType?.Contains("application/json") == true)
                    {
                        context.Request.EnableBuffering();
                        using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                        var body = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;

                        if (Regex.IsMatch(body, @"<[^>]+>"))
                        {
                            await RejectRequest(context, 400, "HTML tags are not allowed in JSON body.");
                            return;
                        }
                    }
                    else if (context.Request.ContentType?.Contains("multipart/form-data") == true)
                    {
                        var form = await context.Request.ReadFormAsync();
                        foreach (var field in form)
                        {
                            if (Regex.IsMatch(field.Value, @"<[^>]+>"))
                            {
                                await RejectRequest(context, 400, $"HTML tags are not allowed in field '{field.Key}'.");
                                return;
                            }
                        }
                        
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                await RejectRequest(context, 500, ex.Message);
            }
        }

        private async Task RejectRequest(HttpContext context, int statusCode, string message)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = ApiResponse<string>.ErrorResponse(message);
            var jsonResponse = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(jsonResponse);
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            string keyPrefix;
            string cacheKey;

            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.User.FindFirst("nameid")?.Value ?? "unknown";
                keyPrefix = "user:";
                cacheKey = $"{keyPrefix}{userId}";
            }
            else
            {
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                keyPrefix = "ip:";
                cacheKey = $"{keyPrefix}{ipAddress}";
            }

            var now = DateTime.UtcNow;
            if (_memoryCache.TryGetValue(cacheKey, out (DateTime timestamp, int count) entry))
            {
                if (now - entry.timestamp < _rateLimitWindow)
                {
                    int limit = keyPrefix == "user:" ? AuthenticatedLimit : GuestLimit;
                    if (entry.count >= limit)
                    {
                        return false;
                    }
                    _memoryCache.Set(cacheKey, (entry.timestamp, entry.count + 1), _rateLimitWindow);
                }
                else
                {
                    _memoryCache.Set(cacheKey, (now, 1), _rateLimitWindow);
                }
            }
            else
            {
                _memoryCache.Set(cacheKey, (now, 1), _rateLimitWindow);
            }

            return true;
        }

        private void ApplySecurityHeaders(HttpContext context)
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
