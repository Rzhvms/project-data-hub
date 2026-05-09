using System.Text.Json.Serialization;

namespace Application.UseCases.Users.Dto.Response;

public record GetUserListResponse
{
    /// <summary>
    /// Список пользователей
    /// </summary>
    [JsonPropertyName("userList")]
    public List<GetUserResponse> UserList { get; init; }
}