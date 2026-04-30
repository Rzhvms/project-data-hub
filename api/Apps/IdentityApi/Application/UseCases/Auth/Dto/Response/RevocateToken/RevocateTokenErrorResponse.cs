using Application.UseCases.Auth.Dto.Response.Base;

namespace Application.UseCases.Auth.Dto.Response.RevocateToken;

/// <summary>
/// Выходная модель деавторизации пользователя с ошибкой
/// </summary>
public record RevocateTokenErrorResponse : RevocateTokenResponse, IBaseErrorResponse
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