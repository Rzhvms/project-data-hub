using System.Text.Json.Serialization;

namespace Application.UseCases.Participants.Dto.Response;

public record GetParticipantListResponse
{
    [JsonPropertyName("participantList")]
    public List<GetParticipantResponse>? ParticipantList { get; init; }
}