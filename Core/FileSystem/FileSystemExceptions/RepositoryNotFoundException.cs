namespace ConsoleApp2.FileSystem.FileSystemExceptions;

public class RepositoryNotFoundException : Exception
{
    public RepositoryNotFoundException(string? message) : base(message)
    {
        Console.WriteLine(message);
    }
}