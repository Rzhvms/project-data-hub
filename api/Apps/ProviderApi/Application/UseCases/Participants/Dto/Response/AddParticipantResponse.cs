namespace Application.UseCases.Participants.Dto.Response;

public record AddParticipantResponse
{
    /// <summary>
    /// Идентификатор участника
    /// </summary>
    public Guid ParticipantId { get; init; }
}