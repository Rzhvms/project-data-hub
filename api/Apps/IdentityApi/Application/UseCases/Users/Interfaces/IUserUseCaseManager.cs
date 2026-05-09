using Application.UseCases.Users.Dto.Request;
using Application.UseCases.Users.Dto.Response;

namespace Application.UseCases.Users.Interfaces;

/// <summary>
/// UseCase сценарии по работе с данными пользователей
/// </summary>
public interface IUserUseCaseManager
{
    /// <summary>
    /// Получить список пользователей
    /// </summary>
    Task<GetUserListResponse> GetUserListAsync();

    /// <summary>
    /// Получить пользователя по идентификатору
    /// </summary>
    Task<GetUserResponse> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// Удалить пользователя по идентификатору
    /// </summary>
    Task DeleteUserByIdAsync(Guid userId);
    
    /// <summary>
    /// Изменение пароля
    /// </summary>
    Task<ChangeUserPasswordResponse> ChangePasswordAsync(Guid id, ChangeUserPasswordRequest request);
}