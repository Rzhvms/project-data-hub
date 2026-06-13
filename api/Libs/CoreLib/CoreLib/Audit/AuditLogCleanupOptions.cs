namespace CoreLib.Audit;

/// <summary>
/// Настройки фоновой очистки журнала аудита
/// </summary>
public class AuditLogCleanupOptions
{
    /// <summary>
    /// Количество дней, которые хранятся записи аудита (env: AuditLogCleanup__RetentionDays). По умолчанию 14
    /// </summary>
    public int RetentionDays { get; init; } = 14;

    /// <summary>
    /// Интервал проверки в часах (env: AuditLogCleanup__CheckIntervalHours). По умолчанию 4
    /// </summary>
    public double CheckIntervalHours { get; init; } = 4;
}
