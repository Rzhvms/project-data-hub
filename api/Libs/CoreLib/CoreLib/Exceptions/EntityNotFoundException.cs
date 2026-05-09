using CoreLib.Exceptions.Base;

namespace CoreLib.Exceptions;

/// <summary>
/// Объект не найден
/// </summary>
public class EntityNotFoundException(string message) : BaseException(message)
{
    public EntityNotFoundException() : this("Объект не найден") { }
}