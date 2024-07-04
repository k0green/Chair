using System.Collections.Concurrent;
using System.Net;

namespace Chair.Middlewares;

public class SecurityMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly ConcurrentDictionary<string, (int Count, DateTime LastRequestTime)> RequestCounts = new();
    private const int Limit = 100;
    private const int TimeWindowSeconds = 60;

    public SecurityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsSqlInjection(context.Request))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("SQL Injection detected.");
            return;
        }

        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        if (clientIp != null)
        {
            var requestInfo = RequestCounts.GetOrAdd(clientIp, (0, DateTime.UtcNow));
            if (requestInfo.Count >= Limit && (DateTime.UtcNow - requestInfo.LastRequestTime).TotalSeconds < TimeWindowSeconds)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("Too many requests.");
                return;
            }
            else if ((DateTime.UtcNow - requestInfo.LastRequestTime).TotalSeconds >= TimeWindowSeconds)
            {
                RequestCounts[clientIp] = (1, DateTime.UtcNow);
            }
            else
            {
                RequestCounts[clientIp] = (requestInfo.Count + 1, requestInfo.LastRequestTime);
            }
        }

        await _next(context);
    }

    private static bool IsSqlInjection(HttpRequest request)
    {
        var forbiddenPatterns = new[] { "SELECT", "INSERT", "DELETE", "UPDATE", "--", ";" };
        if ((from query in request.Query from pattern in forbiddenPatterns
                where query.Value.ToString().Contains(pattern, StringComparison.OrdinalIgnoreCase)
                select query).Any())
        {
            return true;
        }

        return (from form in request.Form from pattern in forbiddenPatterns where
                form.Value.ToString().Contains(pattern, StringComparison.OrdinalIgnoreCase)
            select form).Any();
    }
}