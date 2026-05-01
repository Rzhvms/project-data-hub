using Domain.Entities.IdentityUser;

namespace Application.Ports.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями в базе данных
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Получает пользователя по адресу электронной почты или username
    /// </summary>
    Task<User?> GetUserByEmailOrUsernameAsync(string email, string username);

    /// <summary>
    /// Получает пользователя по адресу электронной почты или username.
    /// Поиск происходит по одному идентификатору.
    /// </summary>
    Task<User?> GetUserByEmailOrUsernameAsync(string identifier);

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
    
    /// <summary>
    /// Получение всех пользователей.
    /// </summary>
    Task<IEnumerable<User>> GetAllUsersAsync();

    /// <summary>
    /// Постраничное получение списка пользователей
    /// </summary>
    Task<IEnumerable<User>> GetPagedUsersAsync(int page, int pageSize);

    /// <summary>
    /// Обновление пароля пользователя
    /// </summary>
    Task ChangeUserPasswordAsync(Guid id, string password, string hashSalt);

    /// <summary>
    /// Удаление пользователя по идентификатору
    /// </summary>
    Task DeleteUserByIdAsync(Guid id);
}