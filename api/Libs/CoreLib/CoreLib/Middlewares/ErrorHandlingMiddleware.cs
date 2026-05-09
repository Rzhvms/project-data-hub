using CoreLib.Exceptions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CoreLib.Middlewares;

/// <summary>
/// Отлавливаем кастомные ошибки
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invoke
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var statusCode = ex is BaseException ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                error = ex.Message,
                type = ex.GetType().Name,
                traceId = context.TraceIdentifier
            });
        }
    }
}