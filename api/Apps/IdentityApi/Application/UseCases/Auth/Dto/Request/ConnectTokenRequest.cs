using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Auth.Dto.Request;

/// <summary>
/// Входная модель авторизации
/// </summary>
public record ConnectTokenRequest
{
    /// <summary>
    /// Логин авторищации (email / username)
    /// </summary>
    [Required]
    [MaxLength(50)]
    [JsonPropertyName("login")]
    public required string Login { get; set; }
    
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [MinLength(8)]
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}