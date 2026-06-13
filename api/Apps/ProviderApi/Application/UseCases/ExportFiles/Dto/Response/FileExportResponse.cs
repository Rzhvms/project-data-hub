namespace Application.UseCases.ExportFiles.Dto.Response;

public record FileExportResponse
{
    /// <summary>
    /// Название файла
    /// </summary>
    public required string FileName { get; init; }
    
    /// <summary>
    /// Тип ответа
    /// </summary>
    public required string ContentType { get; init; }
    
    /// <summary>
    /// Байтовое представление файла
    /// </summary>
    public required byte[] Content { get; init; }
}