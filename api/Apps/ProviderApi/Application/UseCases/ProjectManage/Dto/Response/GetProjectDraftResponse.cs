using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Application.UseCases.ProjectManage.Dto.Response;

public record GetProjectDraftResponse
{
    /// <summary>
    /// Параметры полей в карточке проекта
    /// </summary>
    [JsonPropertyName("projectDraft")]
    public JsonObject? ProjectDraft { get; init; }
}