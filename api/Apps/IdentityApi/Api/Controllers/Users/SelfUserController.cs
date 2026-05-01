using Application.UseCases.Users.Dto.Request;
using Application.UseCases.Users.Dto.Response;
using Application.UseCases.Users.Interfaces;
using CoreLib.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Users;

/// <summary>
/// Контроллер по работе с данными пользователя (от его лица)
/// </summary>
[ApiController]
[Route("api/users/self")]
public class SelfUserController(IUserUseCaseManager userUseCaseManager) : ControllerBase
{
    /// <summary>
    /// Изменить пароль.
    /// </summary>
    [HttpPatch("change-password")] 
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ChangeUserPasswordResponse> ChangeUserPasswordAsync(ChangeUserPasswordRequest request)
    {
        var id = User.GetUserId();
        return await userUseCaseManager.ChangePasswordAsync(id, request);
    }
}