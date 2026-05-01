namespace Application.UseCases.Auth.Dto.Request;

/// <summary>
/// Входная модель обновления токена
/// </summary>
public record RefreshTokenRequest
{
    /// <summary>
    /// Access Token
    /// </summary>
    public string AccessToken { get; set; }
    
    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; }
}