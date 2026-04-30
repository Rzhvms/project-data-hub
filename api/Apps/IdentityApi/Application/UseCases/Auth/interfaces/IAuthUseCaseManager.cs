using Application.UseCases.Auth.Dto.Request;
using Application.UseCases.Auth.Dto.Response;
using Application.UseCases.Auth.Dto.Response.ConnectToken;
using Application.UseCases.Auth.Dto.Response.CreateUser;
using Application.UseCases.Auth.Dto.Response.RevocateToken;

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

    /// <summary>
    /// Авторизация. Получение токенов.
    /// </summary>
    Task<ConnectTokenResponse> ConnectTokenAsync(ConnectTokenRequest request);

    /// <summary>
    /// Деавторизация. Отзыв токена.
    /// </summary>
    Task<RevocateTokenResponse> RevocateRefreshTokenAsync(Guid userId);
}