using Domain.Entities.IdentityUser;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями в базе данных
/// </summary>
public interface IAuthRepository
{
    /// <summary>
    /// Получает пользователя по адресу электронной почты
    /// </summary>
    Task<User?> GetUserByEmailOrUsernameAsync(string email, string username);

    /// <summary>
    /// Получает пользователя по уникальному идентификатору
    /// </summary>
    Task<User?> GetUserByUserIdAsync(Guid userId);

    /// <summary>
    /// Создает нового пользователя
    /// </summary>
    Task CreateUserAsync(User user);

    /// <summary>
    /// Обновляет данные существующего пользователя.
    /// </summary>
    Task UpdateUserAsync(User user);
}