using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.Index.Contracts;

namespace ConsoleApp2.Index.Services;

public class IndexService : IIndexService
{
    private readonly IFileSystemProvider _fileSystemProvider;

    public Dictionary<string, IndexRecord> RecordsByPath { get; } = new();
    public Dictionary<string, List<IndexRecord>> RecordsByHash { get; } = new();
    
    public IndexService(IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
    }

    public void WriteToIndex(IndexRecord item)
    {
        RecordsByPath[item.Path] = item;
        if (RecordsByHash.TryGetValue(item.Hash, out var value))
        {
            value.Add(item);
        }
        else
        {
            RecordsByHash[item.Hash] = new List<IndexRecord> { item };
        }
        var indexPath = _fileSystemProvider.GetRootDirectory()!.IndexFile;
        File.AppendAllText(indexPath,item.ToString());
    }

    public void RemoveFromIndexByPath(string path)
    {
        var recordToRemove = RecordsByPath[path];
        RecordsByPath.Remove(path);
        RecordsByHash[recordToRemove.Hash].Remove(recordToRemove);
        var rewriteRecords = RecordsByPath.Select(record => record.ToString()).ToList();
        File.WriteAllLines(_fileSystemProvider.GetRootDirectory()!.IndexFile,rewriteRecords.ToArray());
    }
    
    public IndexRecord? GetRecordByPath(string path)
    {
        RecordsByPath.TryGetValue(path, out var record);
        return record;
    }

    public List<IndexRecord>? GetRecordsByHash(string hash)
    {
        RecordsByHash.TryGetValue(hash, out var list);
        return list;
    }

    public void Initialize()
    {
        var vcsRootDirectory = _fileSystemProvider.GetRootDirectory();
        if (vcsRootDirectory == null) return;
        var indexPath = _fileSystemProvider.GetRootDirectory()!.IndexFile;
        var allLines = File.ReadAllLines(indexPath);
        foreach (var line in allLines)
        {
            var indexRecord = new IndexRecord(line);
            RecordsByPath.Add(indexRecord.Path,indexRecord);
            if (RecordsByHash.TryGetValue(indexRecord.Hash, out var value))
            {
                value.Add(indexRecord);
            }
            else
            {
                RecordsByHash[indexRecord.Hash] = new List<IndexRecord>{ indexRecord };
            }
        }
    }
}