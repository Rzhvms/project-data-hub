using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLib.Api.Controllers.ControllerTypes.Filters.RequestFilters;

/// <summary>
/// Фильтр системных запросов <see cref="SystemControllerBase"/>
/// </summary>
public class SystemRequestFilter : IAsyncActionFilter
{
    private const string HeaderName = "X-System-Request";
    private const string HeaderValue = "proxy";
    
    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var headers = context.HttpContext.Request.Headers;
        
        if (!headers.TryGetValue(HeaderName, out var value) || value != HeaderValue)
        {
            context.Result = new ForbidResult();
            return;
        }

        await next();
    }
}