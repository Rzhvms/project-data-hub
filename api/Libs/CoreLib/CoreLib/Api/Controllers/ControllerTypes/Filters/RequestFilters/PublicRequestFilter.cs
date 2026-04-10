using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLib.Api.Controllers.ControllerTypes.Filters.RequestFilters;

/// <summary>
/// Фильтр публичных запросов <see cref="PublicControllerBase"/>
/// </summary>
public class PublicRequestFilter : IAsyncActionFilter
{
    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();
    }
}