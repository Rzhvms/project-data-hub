namespace CoreLib.Api.Controllers.ControllerTypes.Policies;

/// <summary>
/// Права доступа контроллеров.
/// </summary>
public static class ControllerPolicies
{
    /// <summary>
    /// Пользовательские запросы.
    /// </summary>
    public const string Client = "client";
    
    /// <summary>
    /// Внутренние запросы.
    /// </summary>
    public const string Internal = "internal";
}