using ConsoleApp2.FileSystem.Contracts;

namespace ConsoleApp2.FileSystem.Services;

public class FileSystemProvider : IFileSystemProvider
{

    private VcsRootDirectory? _vcsRootDirectory;
    
    public VcsRootDirectory? GetRootDirectory()
    {
        if (_vcsRootDirectory != null)
        {
            return _vcsRootDirectory;
        }
        var currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
        while (currentDirectory != null)
        {
            if (IsVcsRootDirectory(currentDirectory.FullName))
            {
                _vcsRootDirectory = new VcsRootDirectory(currentDirectory.FullName);
                return _vcsRootDirectory;
            }
            currentDirectory = Directory.GetParent(currentDirectory.FullName);
        }
        return null;
    }

    public bool IsVcsRootDirectory(string dirPath)
    {
        return Directory.Exists(dirPath + '/' + FileSystemConstants.Name);
    }
}