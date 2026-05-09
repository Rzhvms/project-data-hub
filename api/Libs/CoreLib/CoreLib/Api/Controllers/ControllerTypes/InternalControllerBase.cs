using CoreLib.Api.Controllers.ControllerTypes.Attributes;
using CoreLib.Api.Controllers.ControllerTypes.Base;
using Microsoft.AspNetCore.Authorization;

namespace CoreLib.Api.Controllers.ControllerTypes;

/// <summary>
/// Базовый контроллер для внутренних запросов.
/// </summary>
[InternalController]
[Authorize(Policy = Policies.ControllerPolicies.Client)]
public abstract class InternalControllerBase : ApiControllerBase
{
}