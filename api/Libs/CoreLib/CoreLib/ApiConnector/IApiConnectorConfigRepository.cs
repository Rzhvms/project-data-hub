namespace CoreLib.ApiConnector;

/// <summary>
/// Репозиторий для получения конфигураций Api-коннекторов
/// </summary>
public interface IApiConnectorConfigRepository
{
    /// <summary>
    /// Получение всех активных коннекторов
    /// </summary>
    Task<List<ApiConnectorConfig>> GetActiveAsync();
}
