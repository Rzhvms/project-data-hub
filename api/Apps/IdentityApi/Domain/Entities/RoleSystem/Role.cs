using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.RoleSystem;

/// <summary>
/// Роль пользователя.
/// </summary>
public record Role
{
    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Машинный код роли: Viewer, Editor, Administrator.
    /// </summary>
    [MaxLength(50)]
    public string RoleCode { get; set; } = null!;

    /// <summary>
    /// Название роли.
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Битовая маска прав.
    /// </summary>
    public PermissionList PermissionsMask { get; set; }

    /// <summary>
    /// Системная роль или пользовательская.
    /// </summary>
    public bool IsSystem { get; set; } = true;

    /// <summary>
    /// Дата создания записи.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Дата изменения записи.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}