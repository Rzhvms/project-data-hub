namespace Application.UseCases.Auth.Dto.Response.RefreshToken;

/// <summary>
/// Выходная модель успешного обновления токена
/// </summary>
public record RefreshTokenSuccessResponse : RefreshTokenResponse
{   
    /// <summary>
    /// Access Token
    /// </summary>
    public string AccessToken { get; set; }
    
    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; }
    
    /// <summary>
    /// Дата истечения Refresh токена
    /// </summary>
    public DateTime RefreshTokenExpirationDate { get; set; }
}