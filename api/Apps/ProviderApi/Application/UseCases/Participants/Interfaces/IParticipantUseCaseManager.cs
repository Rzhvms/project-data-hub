using Application.UseCases.Participants.Dto.Request;
using Application.UseCases.Participants.Dto.Response;

namespace Application.UseCases.Participants.Interfaces;

public interface IParticipantUseCaseManager
{
    /// <summary>
    /// Получить список участников
    /// </summary>
    Task<GetParticipantListResponse> GetParticipantsAsync();

    /// <summary>
    /// Получить участника по идентификатору
    /// </summary>
    Task<GetParticipantResponse> GetParticipantAsync(Guid participantId);

    /// <summary>
    /// Удалить участника по идентификатору
    /// </summary>
    Task DeleteParticipantAsync(Guid participantId);

    /// <summary>
    /// Добавить нового участника
    /// </summary>
    Task<AddParticipantResponse> AddParticipantAsync(AddParticipantRequest request);

    /// <summary>
    /// Обновить участника
    /// </summary>
    Task<PatchParticipantResponse> UpdateParticipantAsync(Guid participantId, PatchParticipantRequest request);
}