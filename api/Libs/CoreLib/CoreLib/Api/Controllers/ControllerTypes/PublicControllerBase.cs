using CoreLib.Api.Controllers.ControllerTypes.Attributes;
using CoreLib.Api.Controllers.ControllerTypes.Base;
using Microsoft.AspNetCore.Authorization;

namespace CoreLib.Api.Controllers.ControllerTypes;

/// <summary>
/// Базовый контроллер для публичных запросов.
/// </summary>
[AllowAnonymous]
[PublicController]
public abstract class PublicControllerBase : ApiControllerBase
{
}