namespace Application.UseCases.Auth.Dto.Response.RevocateToken;

/// <summary>
/// Выходная модель успешной деавторизации пользователя
/// </summary>
public record RevocateTokenSuccessResponse : RevocateTokenResponse
{
    /// <summary>
    /// Сообщение о выходе пользователя
    /// </summary>
    public string Message { get; set; } = null!;
}