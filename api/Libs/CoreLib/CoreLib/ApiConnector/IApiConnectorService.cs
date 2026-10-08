namespace CoreLib.ApiConnector;

/// <summary>
/// Сервис для отправки данных во внешние системы через сконфигурированные Api-коннекторы
/// </summary>
public interface IApiConnectorService
{
    /// <summary>
    /// Отправить запрос через коннектор с указанным именем
    /// </summary>
    Task<HttpResponseMessage> SendAsync(string connectorName, object? data = null);
}
