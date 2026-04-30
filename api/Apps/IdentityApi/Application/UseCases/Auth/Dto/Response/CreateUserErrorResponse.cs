namespace Application.UseCases.Auth.Dto.Response;

/// <summary>
/// Выходная модель с неуспешным созданием пользователя
/// </summary>
public record CreateUserErrorResponse : CreateUserResponse
{
    /// <summary>
    /// Текст ошибки
    /// </summary>
    public string Message { get; internal set; }
    
    /// <summary>
    /// Код ошибки
    /// </summary>
    public string Code { get; internal set; }
}