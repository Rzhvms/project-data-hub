using CoreLib.Api.Controllers.ControllerTypes.Attributes;
using CoreLib.Api.Controllers.ControllerTypes.Base;
using Microsoft.AspNetCore.Authorization;

namespace CoreLib.Api.Controllers.ControllerTypes;

/// <summary>
/// Базовый контроллер для пользовательских запросов.
/// </summary>
[ClientController]
[Authorize]
public abstract class ClientControllerBase : ApiControllerBase
{
}