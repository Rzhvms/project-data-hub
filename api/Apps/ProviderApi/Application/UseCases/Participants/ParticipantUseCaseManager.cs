using Application.Ports.Repositories;
using Application.UseCases.Participants.Dto.Request;
using Application.UseCases.Participants.Dto.Response;
using Application.UseCases.Participants.Interfaces;

namespace Application.UseCases.Participants;

internal class ParticipantUseCaseManager(IParticipantRepository repository) : IParticipantUseCaseManager
{
    /// <inheritdoc/>
    public async Task<GetParticipantListResponse> GetParticipantsAsync()
    {
        var response = await repository.GetAllParticipantsAsync();
            
        return new GetParticipantListResponse
        {
            ParticipantList = response.Select(participant => new GetParticipantResponse()
            {
                Id = participant.Id,
                Name = participant.Name,
                Description = participant.Description,
                IsActive = participant.IsActive,
                IsSystem = participant.IsSystem,
                CreatedAt = participant.CreatedAt,
                UpdatedAt = participant.UpdatedAt,
            }).ToList()
        };
    }

    /// <inheritdoc/>
    public async Task<GetParticipantResponse> GetParticipantAsync(Guid participantId)
    {
        var participant = await repository.GetParticipantByIdAsync(participantId);

        return new GetParticipantResponse()
        {
            Id = participant.Id,
            Name = participant.Name,
            Description = participant.Description,
            IsActive = participant.IsActive,
            IsSystem = participant.IsSystem,
            CreatedAt = participant.CreatedAt,
            UpdatedAt = participant.UpdatedAt
        };
    }

    /// <inheritdoc/>
    public async Task<AddParticipantResponse> AddParticipantAsync(AddParticipantRequest request)
    {
        var participantId = await repository.AddParticipantAsync(request.Name, request.Description);
        return new AddParticipantResponse
        {
            ParticipantId = participantId
        };
    }

    /// <inheritdoc/>
    public async Task<PatchParticipantResponse> UpdateParticipantAsync(Guid participantId, PatchParticipantRequest request)
    {
        var participant = await repository.UpdateParticipantAsync(participantId, request.Name, request.Description, request.IsActive);
        return new PatchParticipantResponse()
        {
            Id = participant.Id,
            Name = participant.Name,
            Description = participant.Description,
            IsActive = participant.IsActive,
            IsSystem = participant.IsSystem,
            CreatedAt = participant.CreatedAt,
            UpdatedAt = participant.UpdatedAt
        };
    }

    /// <inheritdoc/>
    public async Task DeleteParticipantAsync(Guid participantId)
    {
        await repository.DeleteParticipantAsync(participantId);
    }
}