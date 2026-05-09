using Application.UseCases.Auth.Dto.Response.Base;

namespace Application.UseCases.Auth.Dto.Response.CreateUser;

/// <summary>
/// Выходная модель с неуспешным созданием пользователя
/// </summary>
public record CreateUserErrorResponse : CreateUserResponse, IBaseErrorResponse
{
    /// <summary>
    /// Текст ошибки
    /// </summary>
    public string? Message { get; set; }
    
    /// <summary>
    /// Код ошибки
    /// </summary>
    public string? Code { get; set; }
}