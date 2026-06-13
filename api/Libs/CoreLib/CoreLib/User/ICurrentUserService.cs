namespace CoreLib.User;

/// <summary>
/// Предоставляет информацию о текущем аутентифицированном пользователе
/// на основе JWT-токена из HTTP-контекста
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Идентификатор текущего пользователя. null для неаутентифицированных запросов
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Полное имя (Фамилия Имя Отчество) текущего пользователя. null для неаутентифицированных запросов
    /// </summary>
    string? UserName { get; }
}
