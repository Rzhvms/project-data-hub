using CoreLib.Exceptions.Base;

namespace CoreLib.Exceptions;

public class InvalidObjectException(string message) : BaseException(message)
{
    public InvalidObjectException() : this("Sorry, object is invalid") { }
}