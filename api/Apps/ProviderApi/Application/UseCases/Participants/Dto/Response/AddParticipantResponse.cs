using System.Text.Json.Serialization;

namespace Application.UseCases.Participants.Dto.Response;

public record AddParticipantResponse
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    [JsonPropertyName("participantId")]
    public Guid ParticipantId { get; init; }
}