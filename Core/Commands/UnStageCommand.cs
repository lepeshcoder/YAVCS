using ConsoleApp2.Blob.Contracts;
using ConsoleApp2.Commands.CommandsExceptions;
using ConsoleApp2.Commands.Contracts;
using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.Index.Contracts;

namespace ConsoleApp2.Commands;

public class UnStageCommand : ICommand
{

    private readonly IFileSystemProvider _fileSystemProvider;
    private readonly IBlobService _blobService;
    private readonly IIndexService _indexService;

    public UnStageCommand(IFileSystemProvider fileSystemProvider, IIndexService indexService, IBlobService blobService)
    {
        _fileSystemProvider = fileSystemProvider;
        _indexService = indexService;
        _blobService = blobService;
    }

    public string Description => "Remove item from staging zone(index)";
    public void Execute(string[] args)
    {
        if (args.Length == 0 || args is ["--help"])
        {
            Console.WriteLine(Description);
            return;
        }

        var vcsRootDirectory = _fileSystemProvider.GetRootDirectory();
        if (vcsRootDirectory == null)
        {
            throw new RepositoryNotFoundException("Repository Not Found");
        }

        var currDir = Environment.CurrentDirectory;
        var itemPath = (currDir + '\\' + args[0]).Replace('/', '\\');;
        if (!File.Exists(itemPath) && !Directory.Exists(itemPath))
        {
            throw new ArgumentException(itemPath + " not Found");
        }

        if (File.Exists(itemPath))
        {
            UnStageFile(itemPath);
        }
        else if (Directory.Exists(itemPath))
        {
            UnStageDirectory(itemPath);
        }
        else
        {
            throw new NotSupportedFileSystemEntryException(itemPath + "isn't file or directory");
        }

        Console.WriteLine(itemPath + "successfully unStaged");
    }

    private void UnStageFile(string itemPath)
    {
        if (!_indexService.IsFileStaged(itemPath))
        {
            throw new ArgumentException("File isn't staged yet");
        }
        var indexRecord = _indexService.GetRecordByPath(itemPath);
        _indexService.RemoveFromIndexByPath(itemPath);
        if (_indexService.GetRecordsByHash(indexRecord!.Hash)!.Count == 1)
        {
            _blobService.DeleteBlob(indexRecord.Hash);
        }
    }

    private void UnStageDirectory(string itemPath)
    {
        foreach (var entryPath in Directory.GetFileSystemEntries(itemPath))
        {
            if (File.Exists(entryPath))
            {
                if(_indexService.IsFileStaged(entryPath))
                    UnStageFile(entryPath);
            }
            else if (Directory.Exists(entryPath))
                UnStageDirectory(entryPath);
            else throw new NotSupportedFileSystemEntryException(entryPath + "isn't file or directory");
        }
    }
}