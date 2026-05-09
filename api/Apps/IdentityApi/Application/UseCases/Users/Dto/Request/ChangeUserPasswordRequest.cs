using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Users.Dto.Request;

/// <summary>
/// Входная модель изменения пароля
/// </summary>
public record ChangeUserPasswordRequest
{
    /// <summary>
    /// Новый пароль
    /// </summary>
    [JsonPropertyName("newPassword")]
    [Required]
    [MinLength(8)]
    public required string NewPassword { get; init; }
    
    /// <summary>
    /// Старый пароль
    /// </summary>
    [JsonPropertyName("oldPassword")]
    [Required]
    [MinLength(8)]
    public required string OldPassword { get; init; }
}