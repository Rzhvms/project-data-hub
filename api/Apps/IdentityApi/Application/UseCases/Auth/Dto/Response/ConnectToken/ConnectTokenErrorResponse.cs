using Application.UseCases.Auth.Dto.Response.Base;

namespace Application.UseCases.Auth.Dto.Response.ConnectToken;

/// <summary>
/// Ответ на неуспешную авторизацию пользователя.
/// Содержит код ошибки и текстовое сообщение для клиента.
/// </summary>
public record ConnectTokenErrorResponse : ConnectTokenResponse, IBaseErrorResponse
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