using Application.UseCases.Auth.Dto.Request;
using Application.UseCases.Auth.Dto.Response;

namespace Application.UseCases.Auth.interfaces;

/// <summary>
/// UseCase сценарии авторизации, аутентификации, регистрации
/// </summary>
public interface IAuthUseCaseManager
{
    /// <summary>
    /// Создание нового пользователя
    /// </summary>
    Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
}