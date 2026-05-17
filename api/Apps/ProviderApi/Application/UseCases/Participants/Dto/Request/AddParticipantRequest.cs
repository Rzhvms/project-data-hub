using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.UseCases.Participants.Dto.Request;

public record AddParticipantRequest
{
    /// <summary>
    /// Отображаемое название участника
    /// </summary>
    [MaxLength(200)]
    [Required]
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// Описание участника
    /// </summary>
    [Required]
    [JsonPropertyName("description")]
    public required string Description { get; set; }
}