namespace CoreLib.ApiConnector;

/// <summary>
/// Настройки одного Api-коннектора для внешней интеграции
/// </summary>
public class ApiConnectorConfig
{
    /// <summary>
    /// Имя коннектора
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// Http-метод
    /// </summary>
    public string HttpMethod { get; init; } = "POST";

    /// <summary>
    /// Шаблон URL
    /// </summary>
    public string UrlTemplate { get; init; } = null!;

    /// <summary>
    /// Заголовки запроса в формате JSON
    /// </summary>
    public string HeadersJson { get; init; } = "{}";

    /// <summary>
    /// Шаблон тела запроса в JSON
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
}
