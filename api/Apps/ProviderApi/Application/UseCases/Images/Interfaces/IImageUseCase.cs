using Application.UseCases.Images.Dto.Request;
using Application.UseCases.Images.Dto.Response;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.Images.Interfaces;

/// <summary>
/// Юзкейс для работы с изображениями
/// </summary>
public interface IImageUseCase
{
    /// <summary>
    /// Добавить файл к проекту
    /// </summary>
    Task AddProjectImageAsync(Guid projectId, UploadProjectImageRequest request);

    /// <summary>
    /// Удалить файл проекта
    /// </summary>
    Task DeleteProjectImageAsync(Guid projectId, Guid imageId);

    /// <summary>
    /// Получить файлы привязанные к проекту
    /// </summary>
    Task<IEnumerable<ProjectImageResponse>> GetProjectImagesAsync(Guid projectId);
}