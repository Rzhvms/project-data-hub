using CoreLib.Api.Controllers.ControllerTypes.Filters.RequestFilters;
using Microsoft.AspNetCore.Mvc;

namespace CoreLib.Api.Controllers.ControllerTypes.Attributes;

/// <summary>
/// Атрибут для <see cref="SystemRequestFilter"/>
/// </summary>
public class SystemControllerAttribute() : TypeFilterAttribute(typeof(SystemRequestFilter));