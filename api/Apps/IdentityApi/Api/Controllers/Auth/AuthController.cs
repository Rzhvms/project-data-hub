using Application.UseCases.Auth.Dto.Request;
using Application.UseCases.Auth.Dto.Response;
using Application.UseCases.Auth.Dto.Response.ConnectToken;
using Application.UseCases.Auth.Dto.Response.CreateUser;
using Application.UseCases.Auth.Dto.Response.RevocateToken;
using Application.UseCases.Auth.interfaces;
using CoreLib.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth;

[ApiController]
[Route("api/connect")]
public class AuthController(IAuthUseCaseManager authUseCaseManager) : ControllerBase
{
    /// <summary>
    /// Создание пользователя / регистрация
    /// </summary>
    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<CreateUserResponse> RegisterAsync(CreateUserRequest request)
    {
        return await authUseCaseManager.CreateUserAsync(request);
    }
    
    /// <summary>
    /// Авторизация пользователя
    /// </summary>
    [AllowAnonymous]
    [HttpPost("token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConnectTokenAsync(ConnectTokenRequest request)
    {
        var response = await authUseCaseManager.ConnectTokenAsync(request);
        return response is ConnectTokenErrorResponse error ? BadRequest(error) : Ok(response);
    }
    
    /// <summary>
    /// Выход пользователя из системы. Отзыв refresh токена
    /// </summary>
    [Authorize]
    [HttpPost("token/revocation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<RevocateTokenResponse> RevocateRefreshTokenAsync()
    {
        var userId = User.GetUserId();
        return await authUseCaseManager.RevocateRefreshTokenAsync(userId);
    }
}