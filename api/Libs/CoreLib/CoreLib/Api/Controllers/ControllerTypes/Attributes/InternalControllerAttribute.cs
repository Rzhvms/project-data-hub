using CoreLib.Api.Controllers.ControllerTypes.Filters.RequestFilters;
using Microsoft.AspNetCore.Mvc;

namespace CoreLib.Api.Controllers.ControllerTypes.Attributes;

/// <summary>
/// Атрибут для <see cref="InternalRequestFilter"/>
/// </summary>
public class InternalControllerAttribute() : TypeFilterAttribute(typeof(InternalRequestFilter));