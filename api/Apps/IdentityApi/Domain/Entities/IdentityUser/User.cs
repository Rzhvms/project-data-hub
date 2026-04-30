using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.IdentityUser;

/// <summary>
/// Модель таблицы пользователей для базы данных.
/// </summary>
public record User
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    [MaxLength(50)]
    public string Username { get; init; } = null!;

    /// <summary>
    /// Электронная почта.
    /// </summary>
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; init; } = null!;

    /// <summary>
    /// Номер телефона.
    /// </summary>
    [MaxLength(50)]
    public string? Phone { get; init; }

    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public string Password { get; init; } = null!;

    /// <summary>
    /// Подтверждена ли почта.
    /// </summary>
    public bool IsEmailConfirmed { get; init; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [MaxLength(50)]
    public string? FirstName { get; init; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    [MaxLength(50)]
    public string? LastName { get; init; }

    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid RoleId { get; init; }

    /// <summary>
    /// Дата создания пользователя.
    /// </summary>
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего обновления данных пользователя.
    /// </summary>
    public DateTime? UpdatedAt { get; init; }

    /// <summary>
    /// Связанный refresh-токен для обновления JWT.
    /// </summary>
    public RefreshToken? RefreshToken { get; set; }
    
    /// <summary>
    /// Конструктор по умолчанию. Инициализирует коллекции.
    /// </summary>
    public User()
    {
        RefreshToken = new RefreshToken();
    }
}