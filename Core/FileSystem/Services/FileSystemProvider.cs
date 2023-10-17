using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.FileSystem.FileSystemExceptions;

namespace ConsoleApp2.FileSystem;

public class FileSystemProvider : IFlieSystemProvider
{
    public VcsRootDirectory? GetRootDirectory()
    {
        var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
        while (currentDirectory != null)
        {
            if (IsVcsRootDirectory(currentDirectory.Name))
            {
                return new VcsRootDirectory(currentDirectory.Name);
            }
            currentDirectory = Directory.GetParent(currentDirectory.Name);
        }
        return null;
    }

    public bool IsVcsRootDirectory(string dirPath)
    {
        return Directory.Exists(dirPath + '/' + FileSystemConstants.Name);
    }
}