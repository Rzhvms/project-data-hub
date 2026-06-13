using CoreLib.Exceptions.Base;

namespace CoreLib.Exceptions;

public class PresentationCreationException(string message) : BaseException(message)
{
    public PresentationCreationException() : this("Error while creating presentation") { }
}