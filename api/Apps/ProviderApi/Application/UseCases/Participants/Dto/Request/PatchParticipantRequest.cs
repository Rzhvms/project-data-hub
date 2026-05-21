using System.Text.Json.Serialization;

namespace Application.UseCases.Participants.Dto.Request;

public record PatchParticipantRequest
{
    /// <summary>
    /// Отображаемое название участника
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; } = null!;

    /// <summary>
    /// Описание участника
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Признак активности участника
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool? IsActive { get; init; } = true;
}