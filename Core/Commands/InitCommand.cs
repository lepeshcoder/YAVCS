using ConsoleApp2.Commands.CommandsExceptions;
using ConsoleApp2.Commands.Contracts;
using ConsoleApp2.FileSystem.Contracts;

namespace ConsoleApp2.Commands;

public class InitCommand : ICommand
{
    private readonly IFlieSystemProvider _fileSystemProvider;
    
    public string Description => "Create a Repository if it doesn't exist"; 
    
    public InitCommand(IFlieSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
    }
    
    public void Execute(string[] args)
    {
        var currentDirectoryPath = Environment.CurrentDirectory;
        if (args[0] == "help")
        {
            Console.WriteLine(Description);
            return;
        }
        if (_fileSystemProvider.IsVcsRootDirectory(currentDirectoryPath))
        {
            throw new RepositoryAlreadyExistsException("Repository Already exists");
        }
        CreateVcsRootDirectory(currentDirectoryPath);
    }

    private static void CreateVcsRootDirectory(string path)
    {
        var vcsRootDirectory = new VcsRootDirectory(path);
        Directory.CreateDirectory(vcsRootDirectory.RootDirectory);
        Directory.CreateDirectory(vcsRootDirectory.RefsDirectory);
        Directory.CreateDirectory(vcsRootDirectory.ObjectsDirectory);
        Directory.CreateDirectory(vcsRootDirectory.BlobsDirectory);
        Directory.CreateDirectory(vcsRootDirectory.CommitsDirectory);
        Directory.CreateDirectory(vcsRootDirectory.TreesDirectory);
        File.Create(vcsRootDirectory.HeadFile);
        File.Create(vcsRootDirectory.IndexFile);
    }

}