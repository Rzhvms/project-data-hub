using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Auth.Dto.Request;

/// <summary>
/// Входная модель создания пользователя
/// </summary>
public record CreateUserRequest
{
    /// <summary>
    /// Электронная почта. Используется в качестве логина.
    /// </summary>
    [EmailAddress]
    [MaxLength(50)]
    [Required]
    [JsonPropertyName("email")]
    public required string Email { get; init; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [MinLength(8)]
    [JsonPropertyName("password")]
    public required string Password { get; init; }
    
    /// <summary>
    /// Имя
    /// </summary>
    [Required]
    [JsonPropertyName("firstName")]
    [MaxLength(50)]
    public required string FirstName { get; init; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    [Required]
    [JsonPropertyName("lastName")]
    [MaxLength(50)]
    public required string LastName { get; init; }
    
    /// <summary>
    /// Отчество пользователя.
    /// </summary>
    [MaxLength(50)]
    [JsonPropertyName("patronymic")]
    public string? Patronymic { get; init; }
    
    /// <summary>
    /// Название роли (Viewer / Editor / Administrator)
    /// </summary>
    [JsonPropertyName("roleName")]
    [Required]
    [AllowedValues("Viewer", "Editor", "Administrator")]
    public required string RoleName { get; init; }
}