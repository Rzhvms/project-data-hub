namespace Application.UseCases.Participants.Dto.Response;

public record GetParticipantListResponse
{
    public List<GetParticipantResponse> ParticipantList { get; init; }
}