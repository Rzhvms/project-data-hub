namespace IdentityLib.Exceptions.Base;

/// <summary>
/// Базовый класс кастомной ошибки.
/// </summary>
public class BaseException : Exception
{
    protected BaseException(string message) : base(message) { }
    protected BaseException(string message, Exception inner) : base(message, inner) { }
}