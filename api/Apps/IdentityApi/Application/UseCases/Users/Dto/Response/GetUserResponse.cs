using System.Text.Json.Serialization;

namespace Application.UseCases.Users.Dto.Response;

public record GetUserResponse
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    [JsonPropertyName("userId")]
    public Guid Id { get; init; }

    /// <summary>
    /// Электронная почта.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; init; } = null!;

    /// <summary>
    /// Идентификатор роли.
    /// </summary>
    [JsonPropertyName("roleId")]
    public Guid RoleId { get; init; }
    
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    [JsonPropertyName("firstName")]
    public string? FirstName { get; init; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    [JsonPropertyName("lastName")]
    public string? LastName { get; init; }
    
    /// <summary>
    /// Отчество пользователя.
    /// </summary>
    [JsonPropertyName("patronymic")]
    public string? Patronymic { get; init; }
}