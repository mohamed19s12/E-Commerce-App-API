using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _RateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache)
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                ApplySecurityHeaders(context);

                //Too many Requests Handling
                if (!IsRequestAllowed(context))
                {
                    // Return 429 Too Many Requests
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";

                    var response = 
                        new ResponseAPI((int)HttpStatusCode.TooManyRequests, "Too many requests. Please try again later.");

                    await context.Response.WriteAsJsonAsync(response);
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                
                var response = _environment.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode.TooManyRequests, ex.Message , ex.StackTrace)
                    : new ResponseAPI((int)HttpStatusCode.TooManyRequests, ex.Message);


                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);

            }
        }
        // Handle rate limiting Like Bot requests
        private bool IsRequestAllowed(HttpContext context)
        {
            var userIp = context.Connection.RemoteIpAddress?.ToString();
            var cacheKey = $"Rate:{userIp}";
            var dateNow = DateTime.Now;

            var (timesTamp, count) = _memoryCache.GetOrCreate(userIp , entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _RateLimitWindow;
                return (timesTamp:dateNow , count:0 );
            });

            // If within the rate limit window  <LogIn>
            if ((dateNow - timesTamp) < _RateLimitWindow)
            {
                if (count >= 8)
                {
                    return false;
                }
                _memoryCache.Set(cacheKey, (timesTamp, count += 1), _RateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cacheKey, (dateNow, count), _RateLimitWindow);
            }
            return true;
        }

        private void ApplySecurityHeaders(HttpContext context)
        {
            // protection from txt file
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            // protection from clickjacking
            context.Response.Headers["X-Frame-Options"] = "DENY";
            // protection from XSS attacks
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            
            //context.Response.Headers["Referrer-Policy"] = "no-referrer");
            //context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self'");
        }
    }
}
