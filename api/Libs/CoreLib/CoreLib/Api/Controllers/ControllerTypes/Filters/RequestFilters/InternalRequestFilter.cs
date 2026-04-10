using Microsoft.AspNetCore.Mvc.Filters;

namespace CoreLib.Api.Controllers.ControllerTypes.Filters.RequestFilters;

/// <summary>
/// Фильтр внутренних запросов <see cref="InternalControllerBase"/>
/// </summary>
public class InternalRequestFilter : IAsyncActionFilter
{
    /// <inheritdoc />
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await next();
    }
}