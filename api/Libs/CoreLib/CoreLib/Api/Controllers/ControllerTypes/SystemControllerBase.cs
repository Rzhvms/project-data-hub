using CoreLib.Api.Controllers.ControllerTypes.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace CoreLib.Api.Controllers.ControllerTypes;

/// <summary>
/// Базовый контроллер для системных инфраструктурных запросов.
/// </summary>
[ApiController]
[SystemController]
[ApiExplorerSettings(IgnoreApi = true)]
public abstract class SystemControllerBase : ControllerBase
{
}