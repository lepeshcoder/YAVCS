namespace ConsoleApp2.Commands.CommandsExceptions;

public class NotSupportedFileSystemEntryException : Exception
{
    public NotSupportedFileSystemEntryException(string? message) : base(message)
    {
        
    }
}