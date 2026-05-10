using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Auth.Dto.Request;

/// <summary>
/// Входная модель авторизации
/// </summary>
public record ConnectTokenRequest
{
    /// <summary>
    /// Электронная почта, по которой будет произведена авторизация
    /// </summary>
    [Required]
    [MaxLength(50)]
    [JsonPropertyName("email")]
    [EmailAddress]
    public required string Email { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [MinLength(8)]
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}