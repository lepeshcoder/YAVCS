namespace ConsoleApp2.Commands.CommandsExceptions;

public class RepositoryAlreadyExistsException : Exception
{
    public RepositoryAlreadyExistsException(string? message) : base(message)
    {
        
    }
}