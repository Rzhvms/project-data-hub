using Application.UseCases.Auth.Dto.Response.Base;

namespace Application.UseCases.Auth.Dto.Response.RefreshToken;

/// <summary>
/// Выходная модель обновления токена с ошибкой
/// </summary>
public record RefreshTokenErrorResponse : RefreshTokenResponse, IBaseErrorResponse
{
    /// <summary>
    /// Текстовое сообщение с описанием ошибки.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Код ошибки.
    /// </summary>
    public string? Code { get; set; }
}