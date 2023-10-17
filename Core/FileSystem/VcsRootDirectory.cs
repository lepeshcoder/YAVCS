using System.Reflection.Metadata;

namespace ConsoleApp2;

public class VcsRootDirectory
{

    private readonly string _absolutePath;

    public VcsRootDirectory(string absolutePath)
    {
        _absolutePath = absolutePath;
    }

    public string RootDirectory => _absolutePath + '/' + FileSystemConstants.Name;
    public string ObjectsDirectory => RootDirectory + '/' + FileSystemConstants.Objects;
    public string RefsDirectory => RootDirectory + '/' + FileSystemConstants.Refs;
    public string BlobsDirectory => ObjectsDirectory + '/' + FileSystemConstants.Blobs;
    public string TreesDirectory => ObjectsDirectory + '/' + FileSystemConstants.Trees;
    public string CommitsDirectory => ObjectsDirectory + '/' + FileSystemConstants.Commits;
    public string IndexFile => RootDirectory + '/' + FileSystemConstants.Index;
    public string HeadFile => RootDirectory + '/' + FileSystemConstants.Head;
}