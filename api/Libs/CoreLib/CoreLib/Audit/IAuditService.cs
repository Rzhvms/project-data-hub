namespace CoreLib.Audit;

/// <summary>
/// Сервис для записи и чтения действий пользователей в журнале аудита
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Записать событие в журнал аудита
    /// </summary>
    Task LogAsync(string action, string entityType, Guid? entityId = null, Guid? userId = null, string? userName = null, string? details = null);

    /// <summary>
    /// Получить все записи аудита, отсортированные от новых к старым
    /// </summary>
    /// <param name="limit">
    /// Максимальное количество записей (null — без ограничения)
    /// </param>
    Task<IEnumerable<AuditLog>> GetLogsAsync(int? limit = null);
}
