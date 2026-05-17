using Application.UseCases.Participants.Dto.Request;
using Application.UseCases.Participants.Dto.Response;
using Application.UseCases.Participants.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Participants;

[Authorize]
[ApiController]
[Route("api/participant")]
public class ParticipantController(IParticipantUseCaseManager useCaseManager) : ControllerBase
{
    /// <summary>
    /// Получить список доступных участников
    /// </summary>
    [HttpGet("list")]
    [ProducesResponseType(typeof(GetParticipantListResponse), 200)]
    [ProducesResponseType(typeof(GetParticipantListResponse), 400)]
    public async Task<GetParticipantListResponse> GetParticipantsAsync()
    {
        return await useCaseManager.GetParticipantsAsync();
    }

    /// <summary>
    /// Получить участника по идентификатору
    /// </summary>
    [HttpGet("{participantId}")]
    [ProducesResponseType(typeof(GetParticipantResponse), 200)]
    [ProducesResponseType(typeof(GetParticipantResponse), 400)]
    public async Task<GetParticipantResponse> GetParticipantAsync([FromRoute] Guid participantId)
    {
        return await useCaseManager.GetParticipantAsync(participantId);
    }
    
    /// <summary>
    /// Добавить нового участника
    /// </summary>
    [HttpPost("create")]
    [ProducesResponseType(typeof(AddParticipantResponse), 200)]
    [ProducesResponseType(typeof(AddParticipantResponse), 400)]
    public async Task<AddParticipantResponse> AddParticipantAsync(AddParticipantRequest request)
    {
        return await useCaseManager.AddParticipantAsync(request);
    }

    /// <summary>
    /// Обновить данные участника
    /// </summary>
    [HttpPatch("{participantId}")]
    [ProducesResponseType(typeof(PatchParticipantResponse), 200)]
    [ProducesResponseType(typeof(PatchParticipantResponse), 400)]
    public async Task<PatchParticipantResponse> UpdateParticipantAsync([FromRoute] Guid participantId, PatchParticipantRequest request)
    {
        return await useCaseManager.UpdateParticipantAsync(participantId, request);
    }
    
    /// <summary>
    /// Удалить участника по идентификатору
    /// </summary>
    [HttpDelete("delete/{participantId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task DeleteParticipantAsync([FromRoute] Guid participantId)
    {
        await useCaseManager.DeleteParticipantAsync(participantId);
    }
}