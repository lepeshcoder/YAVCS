using System.Net.Http.Json;
using ConsoleApp2.Commands.CommandsExceptions;
using ConsoleApp2.Commands.Contracts;
using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.Hash.Contracts;
using ConsoleApp2.Index.Contracts;

namespace ConsoleApp2.Commands;

public class StatusCommand : ICommand
{

    private readonly IIndexService _indexService;
    private readonly IFileSystemProvider _fileSystemProvider;
    private readonly IHashService _hashService;

    public StatusCommand(IIndexService indexService, IFileSystemProvider fileSystemProvider, IHashService hashService)
    {
        _indexService = indexService;
        _fileSystemProvider = fileSystemProvider;
        _hashService = hashService;
    }

    public string Description => "View tracked/untracked/modified files";

    public void Execute(string[] args)
    {
        if (args is ["--help"])
        {
            Console.WriteLine(Description);
            return;
        }

        var vcsRootDirectory = _fileSystemProvider.GetRootDirectory();
        if (vcsRootDirectory == null)
        {
            throw new RepositoryNotFoundException("Repository not found");
        }

        Console.WriteLine(GetStatus());
    }

    private string GetStatus()
    {
        var indexRecords = _indexService.RecordsByPath;

        var stagedItems = new List<string>();
        var unStagedItems = new List<string>();
        
        foreach (var indexRecord in indexRecords)
        {
            var itemPath = indexRecord.Value.Path;
            if (!File.Exists(itemPath))
            {
                unStagedItems.Add("Deleted: " + itemPath);
            }
            else
            {
                var data = File.ReadAllBytes(itemPath);
                var newHash = _hashService.GetHash(data);
                if (indexRecord.Value.Hash != newHash)
                {
                    unStagedItems.Add("Modified: " + itemPath);
                }
                else
                {
                    stagedItems.Add(itemPath);
                }
            }
        }

        var vcsRootDirectoryPath = _fileSystemProvider.GetRootDirectory()!.RootDirectory;
        var repositoryRootDirectory = Directory.GetParent(vcsRootDirectoryPath)?.FullName;
        var unTrackedItems = GetDirectoryStatus(repositoryRootDirectory!);

        var response = stagedItems.Aggregate("\nStaged files:\n", (current, stagedItem) => current + (stagedItem + "\n"));
        response += "\nUnStaged Files:\n";
        response = unStagedItems.Aggregate(response, (current, unStagedItem) => current + (unStagedItem + "\n"));
        response += "\nUnTracked Files:\n";
        response = unTrackedItems.Aggregate(response, (current, unTrackedItem) => current + (unTrackedItem + "\n"));

        return response;
    }

    private List<string> GetDirectoryStatus(string dirPath,bool isRoot = true)
    {
        var status = new List<string>();
        var isDirHidden = true;
        foreach (var entry in Directory.GetFileSystemEntries(dirPath))
        {
            if (Directory.Exists(entry))
            {
                var subDirStatus = GetDirectoryStatus(entry, false);
                if (subDirStatus.Count == 0) isDirHidden = false;
                else if (!Directory.Exists(subDirStatus[0])) isDirHidden = false;
                status.AddRange(subDirStatus);
            }
            else if(File.Exists(entry))
            {
                if (!_indexService.IsFileStaged(entry))
                    status.Add(entry);
                else isDirHidden = false;
            }
        }
        if (isRoot) return status;
        return isDirHidden ? new List<string>{dirPath} : status;
    }


}

