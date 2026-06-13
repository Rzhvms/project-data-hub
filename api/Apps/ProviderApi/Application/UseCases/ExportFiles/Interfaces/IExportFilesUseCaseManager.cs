using Application.UseCases.ExportFiles.Dto.Request;
using Application.UseCases.ExportFiles.Dto.Response;

namespace Application.UseCases.ExportFiles.Interfaces;

public interface IExportFilesUseCaseManager
{
    /// <summary>
    /// Выгрузка презентации
    /// </summary>
    Task<FileExportResponse> ExportPresentationAsync(ExportPresentationRequest request);
    
    /// <summary>
    /// Выгрузка портфолио
    /// </summary>
    Task<FileExportResponse> ExportPortfolioAsync(ExportPortfolioRequest request);
}