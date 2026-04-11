using IdentityLib.Exceptions.Base;

namespace IdentityLib.Exceptions;

/// <summary>
/// Ошибка невалидного JWT-токена.
/// </summary>
public class InvalidTokenException : BaseException
{
    public InvalidTokenException(string message) : base(message) { }
    public InvalidTokenException(string message, Exception innerException) : base(message, innerException) { }
}