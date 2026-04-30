using Application.UseCases.Auth.Dto.Request;
using Application.UseCases.Auth.Dto.Response;
using Application.UseCases.Auth.interfaces;
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
}