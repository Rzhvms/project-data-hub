namespace Domain.Entities.ApiConnector;

/// <summary>
/// Конфигурация коннектора для внешних интеграций
/// </summary>
public sealed record ApiConnectorConfigEntity
{
    public Guid Id { get; init; }

    /// <summary>
    /// Имя коннектора
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// http-метод
    /// </summary>
    public string HttpMethod { get; init; } = "POST";

    /// <summary>
    /// Шаблон URL с поддержкой плейсхолеров
    /// </summary>
    public string UrlTemplate { get; init; } = null!;

    /// <summary>З
    /// аголовки запроса в формате JSON
    /// </summary>
    public string HeadersJson { get; init; } = "{}";

    /// <summary>
    /// Шаблон тела запроса в формате JSON с плейсхолерами
    /// </summary>
    public string? BodyTemplate { get; init; }

    /// <summary>
    /// Content-Type запроса
    /// </summary>
    public string ContentType { get; init; } = "application/json";

    /// <summary>
    /// Включен ли коннектор
    /// </summary>
    public bool IsActive { get; init; } = true;

    public DateTime CreatedAt { get; init; }

    public DateTime? UpdatedAt { get; init; }
}
