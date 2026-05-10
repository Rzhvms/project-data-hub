using Application.UseCases.Auth.Dto.Request;
using Application.UseCases.Auth.Dto.Response.ConnectToken;
using Application.UseCases.Auth.Dto.Response.CreateUser;
using Application.UseCases.Auth.Dto.Response.RefreshToken;
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
    [HttpPost("token/revoke")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevocateRefreshTokenAsync()
    {
        var userId = User.GetUserId();
        var response = await authUseCaseManager.RevocateRefreshTokenAsync(userId);
        return response is RevocateTokenErrorResponse error ? BadRequest(error) : Ok(response);
    }
    
    /// <summary>
    /// Обновление Refresh токена
    /// </summary>
    [AllowAnonymous]
    [HttpPost("token/refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var response = await authUseCaseManager.RefreshTokenAsync(request);
        return response is RefreshTokenErrorResponse error ? BadRequest(error) : Ok(response);
    }
    
    /// <summary>
    /// Изменение пароля
    /// </summary>
    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task ChangePasswordAsync(ChangePasswordRequest request)
    {
        var userId = User.GetUserId();
        await authUseCaseManager.ChangePasswordAsync(userId, request);
    }
}