using System.Text.Json.Serialization;

namespace Application.UseCases.Auth.Dto.Response.ConnectToken;

/// <summary>
/// Ответ на успешную авторизацию пользователя.
/// Содержит необходимые токены для дальнейшей работы с API.
/// </summary>
public record ConnectTokenSuccessResponse : ConnectTokenResponse
{
    /// <summary>
    /// JWT access-токен для доступа к защищенным ресурсам API.
    /// </summary>
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; }

    /// <summary>
    /// Refresh-токен, используемый для получения нового access-токена после истечения срока действия.
    /// </summary>
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; }
}