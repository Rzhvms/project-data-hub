using CoreLib.Exceptions.Base;

namespace CoreLib.Exceptions;

public class InvalidProjectCardException(string message) : BaseException(message)
{
    public InvalidProjectCardException() : this("Invalid Project Card") { }
}