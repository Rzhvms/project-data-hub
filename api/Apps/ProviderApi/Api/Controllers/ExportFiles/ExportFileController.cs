using Application.UseCases.ExportFiles.Dto.Request;
using Application.UseCases.ExportFiles.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ExportFiles;

/// <summary>
/// Контроллер для выгрузки данных для презентации и портфолио 
/// </summary>
[ApiController]
[Route("api/export")]
public class ExportFileController(IExportFilesUseCaseManager exportUseCase) : ControllerBase
{
    /// <summary>
    /// Выгрузка презентации по проекту
    /// </summary>
    [HttpGet("projects/{projectId}/presentation")]
    public async Task<IActionResult> ExportPresentation([FromRoute] Guid projectId)
    {
        var result = await exportUseCase.ExportPresentationAsync(new ExportPresentationRequest
        {
            ProjectId = projectId
        });

        return File(result.Content, result.ContentType, result.FileName);
    }

    /// <summary>
    /// Выгрузка портфолио по проекту
    /// </summary>
    [HttpPost("projects/{projectId}/portfolio")]
    public async Task<IActionResult> ExportPortfolio([FromRoute] Guid projectId)
    {
        var request = new ExportPortfolioRequest
        {
            ProjectId = projectId
        };

        var result = await exportUseCase.ExportPortfolioAsync(request);

        return File(result.Content, result.ContentType, result.FileName);
    }
}