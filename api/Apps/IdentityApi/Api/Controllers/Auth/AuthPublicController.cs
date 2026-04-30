using CoreLib.Api.Controllers.ControllerTypes;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth;

/// <summary>
/// Публичный контроллер авторизации и аутенификации.
/// </summary>
[Route("connect")]
public class AuthPublicController : PublicControllerBase
{
    public AuthPublicController()
    {
    }

    /// <summary>
    /// Получение access-токена.
    /// </summary>
    [HttpPost("token")]
    public async Task GetAccessTokenAsync()
    {
        
    }
}