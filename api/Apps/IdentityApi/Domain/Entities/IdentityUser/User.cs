using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.IdentityUser;

/// <summary>
/// Модель пользователя.
/// </summary>
public record User
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Электронная почта.
    /// </summary>
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; init; } = null!;
    
    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public string Password { get; init; } = null!;
    
    /// <summary>
    /// Дополнительная hash-строка, используемая при генерации пароля.
    /// </summary>
    public string HashSalt { get; init; } = null!; // TODO: убрать // МОЖНО ПОЛУЧИТЬ ИЗ ХЭША ПАРОЛЯ

    /// <summary>
    /// Подтверждена ли почта.
    /// </summary>
    public bool IsEmailConfirmed { get; init; }

    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid RoleId { get; init; }
    
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
    /// Отчество пользователя.
    /// </summary>
    [MaxLength(50)]
    public string? Patronymic { get; init; }

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