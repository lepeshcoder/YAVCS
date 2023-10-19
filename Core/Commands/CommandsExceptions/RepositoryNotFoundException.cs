namespace ConsoleApp2.Commands.CommandsExceptions;

public class RepositoryNotFoundException : Exception
{
    public RepositoryNotFoundException(string? message) : base(message)
    {
    }
}