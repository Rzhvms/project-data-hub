namespace Application.UseCases.Participants.Dto.Request;

public record AddParticipantRequest
{
    /// <summary>
    /// Отображаемое название участника
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Описание участника
    /// </summary>
    public required string Description { get; set; }
}