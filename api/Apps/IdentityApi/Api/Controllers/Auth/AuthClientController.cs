using CoreLib.Api.Controllers.ControllerTypes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth;

/// <summary>
/// Пользовательский контроллер авторизации и аутенификации.
/// </summary>
[Route("auth")]
public class AuthClientController : ClientControllerBase
{
    [HttpGet]
    public ActionResult GetSmth()
    {
        return Ok();
    }
}