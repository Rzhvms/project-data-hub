using System.ComponentModel.DataAnnotations;

namespace Application.UseCases.Auth.Dto.Request;

/// <summary>
/// Модель изменения пароля
/// </summary>
public record ChangePasswordRequest
{
    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [MinLength(8)]
    public required string Password { get; init; }
}