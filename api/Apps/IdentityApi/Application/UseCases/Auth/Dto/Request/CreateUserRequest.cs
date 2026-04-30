using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Auth.Dto.Request;

/// <summary>
/// Входная модель создания пользователя
/// </summary>
public record CreateUserRequest
{
    /// <summary>
    /// Логин пользователя.
    /// </summary>
    [MaxLength(50)]
    [Required]
    [JsonPropertyName("userName")]
    public required string Username { get; init; }
    
    /// <summary>
    /// Электронная почта
    /// </summary>
    [EmailAddress]
    [MaxLength(50)]
    [Required]
    [JsonPropertyName("email")]
    public required string Email { get; init; }
    
    /// <summary>
    /// Номер телефона
    /// </summary>
    [MaxLength(20)]
    [JsonPropertyName("phone")]
    [Required]
    public required string Phone { get; set; }
    
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
    [JsonPropertyName("firstName")]
    [Required]
    public required string FirstName { get; init; }
    
    /// <summary>
    /// Фамилия
    /// </summary>
    [JsonPropertyName("lastName")]
    [Required]
    public required string LastName { get; init; }
    
    /// <summary>
    /// Название роли (Viewer / Editor / Administrator)
    /// </summary>
    [JsonPropertyName("roleName")]
    [Required]
    [AllowedValues("Viewer", "Editor", "Administrator")]
    public required string RoleName { get; init; }
}