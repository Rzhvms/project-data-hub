using Domain.Entities.Files;

namespace Application.Services.Generators.Interfaces;

/// <summary>
/// Генератор PPTX-презентации
/// </summary>
public interface IPresentationDocumentGenerator
{
    /// <summary>
    /// Генерирует презентацию
    /// </summary>
    Task<byte[]> GenerateAsync(ProjectExportModel model);
}