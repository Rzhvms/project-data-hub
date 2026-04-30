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
    public Guid Id { get; set; }

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    [MaxLength(50)]
    public string Username { get; set; }

    /// <summary>
    /// Электронная почта.
    /// </summary>
    [EmailAddress]
    [MaxLength(50)]
    public string Email { get; set; }

    /// <summary>
    /// Номер телефона.
    /// </summary>
    [MaxLength(50)]
    public string? Phone { get; set; }

    /// <summary>
    /// Хэш пароля пользователя.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Дополнительный хэш, используемый для пароля.
    /// </summary>
    public string SaltHash { get; set; }

    /// <summary>
    /// Подтверждена ли почта.
    /// </summary>
    public bool IsEmailConfirmed { get; set; } = false;

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [MaxLength(50)]
    public string? FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    [MaxLength(50)]
    public string? LastName { get; set; }

    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Дата создания пользователя.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата последнего обновления данных пользователя.
    /// </summary>
    public DateTime? UpdateAt { get; set; }

    /// <summary>
    /// Связанный refresh-токен для обновления JWT.
    /// </summary>
    public RefreshToken? RefreshToken { get; set; }
}