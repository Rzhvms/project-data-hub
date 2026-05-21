using System.Text.Json.Serialization;

namespace Application.UseCases.Participants.Dto.Response;

public record GetParticipantResponse
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    /// <summary>
    /// Отображаемое название участника
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = null!;

    /// <summary>
    /// Описание участника
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
    
    /// <summary>
    /// Признак активности участника
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; init; }
    
    /// <summary>
    /// Признак системного участника
    /// </summary>
    [JsonPropertyName("isSystem")]
    public bool IsSystem { get; init; }

    /// <summary>
    /// Дата создания
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; }

    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; init; }
}