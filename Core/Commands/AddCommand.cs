using System.IO.Enumeration;
using System.Reflection.PortableExecutable;
using ConsoleApp2.Blob.Contracts;
using ConsoleApp2.Commands.CommandsExceptions;
using ConsoleApp2.Commands.Contracts;
using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.Hash.Contracts;
using ConsoleApp2.Index;
using ConsoleApp2.Index.Contracts;
using RepositoryNotFoundException = ConsoleApp2.FileSystem.FileSystemExceptions.RepositoryNotFoundException;

namespace ConsoleApp2.Commands;

public class AddCommand : ICommand
{

    private readonly IFileSystemProvider _fileSystemProvider;
    private readonly IIndexService _indexService;
    private readonly IBlobService _blobService;
    private readonly IHashService _hashService;
    
    public AddCommand(IFileSystemProvider fileSystemProvider, IIndexService indexService,
        IBlobService blobService, IHashService hashService)
    {
        _fileSystemProvider = fileSystemProvider;
        _indexService = indexService;
        _blobService = blobService;
        _hashService = hashService;
    }

    public string Description => "Add Files or directories into staging zone(index)";
    public void Execute(string[] args)
    {
        if (args.Length > 0 && args[0] == "help")
        {
            Console.WriteLine(Description);
            return;
        }
        
        var vcsRootDirectory = _fileSystemProvider.GetRootDirectory();
        if (vcsRootDirectory is null)
        {
            throw new RepositoryNotFoundException("Repository not found");
        }

        var currDir = Environment.CurrentDirectory;
        var itemToStage = currDir + '/' + args[0];
        if (!File.Exists(itemToStage) && !Directory.Exists(itemToStage))
        {
            throw new ArgumentException(itemToStage + " not Found");
        }
        
        if(File.Exists(itemToStage))
            StageFile(itemToStage);
        else if (Directory.Exists(itemToStage))
            StageDirectory(itemToStage);
        else throw new NotSupportedFileSystemEntryException("item isn't file or directory");
    }

    private void StageFile(string filePath)
    {
        var byteArray = File.ReadAllBytes(filePath);
        var newHash = _hashService.GetHash(byteArray);                     // Calculate newHash
        var oldRecord = _indexService.GetRecordByPath(filePath);               // Find old record
        
        if (oldRecord != null)                                                 // if File already added previosuly 
        {
            if (newHash == oldRecord.Hash)                                     // if File isn't modify
            {
                throw new ArgumentException("Item is already staged");
            }
            else                                                               // if File modified
            {
                var newRecord = new IndexRecord(oldRecord.Path, newHash, oldRecord.Attributes);  
                _indexService.RemoveFromIndexByPath(oldRecord.Path);
                _indexService.WriteToIndex(newRecord);
                
                
                if (_indexService.GetRecordsByHash(oldRecord.Hash)!.Count == 1)  // if blob nas no reference can delete it
                {
                    _blobService.DeleteBlob(oldRecord.Hash);
                }
                if (!_blobService.IsBlobExist(newHash))                         // if blob doesn't exist create it
                {
                    _blobService.WriteBlob(byteArray);   
                }
            }
        }
        else                                                                        // if file not staged yet
        {
            if (!_blobService.IsBlobExist(newHash))
            {
                _blobService.WriteBlob(byteArray);
            }
            var newRecord = new IndexRecord(filePath, newHash, File.GetAttributes(filePath));
            _indexService.WriteToIndex(newRecord);
        }
    }

    private void StageDirectory(string dirPath)
    {
        foreach (var itemPath in Directory.GetFileSystemEntries(dirPath))
        {
            if(File.Exists(itemPath))
                StageFile(itemPath);
            else if (Directory.Exists(itemPath))
                StageDirectory(itemPath);
            else throw new NotSupportedFileSystemEntryException(itemPath + "isn't file or directory");
        }
    }
}