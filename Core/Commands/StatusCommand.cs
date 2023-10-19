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
        if (args.Length > 0 && args[0] == "help")
        {
            Console.WriteLine(Description);
            return;
        }

        var vcsRootDirectory = _fileSystemProvider.GetRootDirectory();
        if (vcsRootDirectory == null)
        {
            throw new RepositoryNotFoundException("Repository not found");
        }
        
        ShowStatus();
    }

    private void ShowStatus()
    {
        var indexRecords = _indexService.RecordsByPath;

        var stagedItems = new List<string>();
        var unStagedItems = new List<string>();
        var unTrackedItems = new List<string>();

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

        foreach (var itemPath in Directory.GetFileSystemEntries(repositoryRootDirectory!))
        {
            if (!indexRecords.ContainsKey(itemPath))
                unTrackedItems.Add(itemPath);
        }

        var response = stagedItems.Aggregate("Staged files:\n", (current, stagedItem) => current + (stagedItem + "\n"));
        response += "UnStaged Files:\n";
        response = unStagedItems.Aggregate(response, (current, unStagedItem) => current + (unStagedItem + "\n"));
        response += "UnTracked Files:\n";
        response = unTrackedItems.Aggregate(response, (current, unTrackedItem) => current + (unTrackedItem + "\n"));

        Console.WriteLine(response);
    }
}