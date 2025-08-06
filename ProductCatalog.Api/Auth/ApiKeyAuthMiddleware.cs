// ADD THIS LINE
using Microsoft.Extensions.Primitives;

namespace ProductCatalog.Api.Auth
{
    public class ApiKeyAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public ApiKeyAuthMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!TryGetHeader(context.Request.Headers, out var suppliedKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("ApiKey header missing.");
                return;
            }

            var expected = _config.GetValue<string>("ApiKey") ?? string.Empty;
            if (!String.Equals(expected, suppliedKey, StringComparison.Ordinal))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid ApiKey.");
                return;
            }

            await _next(context);
        }

        private static bool TryGetHeader(IHeaderDictionary headers, out StringValues value)
        {
            return headers.TryGetValue("ApiKey", out value) ||
                   headers.TryGetValue("apikey", out value) ||
                   headers.TryGetValue("X-API-KEY", out value);
        }
    }
}
