using Application.UseCases.Users.Dto.Request;
using Application.UseCases.Users.Dto.Response;

namespace Application.UseCases.Users.Interfaces;

/// <summary>
/// UseCase сценарии по работе с данными пользователей
/// </summary>
public interface IUserUseCaseManager
{
    /// <summary>
    /// Изменение пароля
    /// </summary>
    Task<ChangeUserPasswordResponse> ChangePasswordAsync(Guid id, ChangeUserPasswordRequest request);
}