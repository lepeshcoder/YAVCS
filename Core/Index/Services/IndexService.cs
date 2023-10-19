using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.Index.Contracts;

namespace ConsoleApp2.Index.Services;

public class IndexService : IIndexService
{
    private readonly IFileSystemProvider _fileSystemProvider;
    
    private readonly Dictionary<string, IndexRecord> _recordsByPath = new();

    private readonly Dictionary<string, List<IndexRecord>> _recordsByHash = new();
    
    public IndexService(IFileSystemProvider fileSystemProvider)
    {
        _fileSystemProvider = fileSystemProvider;
        var indexPath = _fileSystemProvider.GetRootDirectory()!.IndexFile;
        var allLines = File.ReadAllLines(indexPath);
        foreach (var line in allLines)
        {
            var indexRecord = new IndexRecord(line);
            _recordsByPath.Add(indexRecord.Path,indexRecord);
            _recordsByHash[indexRecord.Hash].Add(indexRecord);
        }
    }

    public void WriteToIndex(IndexRecord item)
    {
        _recordsByPath[item.Path] = item;
        _recordsByHash[item.Hash].Add(item);
        if (_recordsByHash.TryGetValue(item.Hash, out var value))
        {
            value.Add(item);
        }
        else
        {
            _recordsByHash[item.Hash] = new List<IndexRecord> { item };
        }
        var indexPath = _fileSystemProvider.GetRootDirectory()!.IndexFile;
        File.AppendAllText(indexPath,item.ToString());
    }

    public void RemoveFromIndexByPath(string path)
    {
        var recordToRemove = _recordsByPath[path];
        _recordsByPath.Remove(path);
        _recordsByHash[recordToRemove.Hash].Remove(recordToRemove);
        var rewriteRecords = _recordsByPath.Select(record => record.ToString()).ToList();
        File.WriteAllLines(_fileSystemProvider.GetRootDirectory()!.IndexFile,rewriteRecords.ToArray());
    }
    
    public IndexRecord? GetRecordByPath(string path)
    {
        _recordsByPath.TryGetValue(path, out var record);
        return record;
    }

    public List<IndexRecord>? GetRecordsByHash(string hash)
    {
        _recordsByHash.TryGetValue(hash, out var list);
        return list;
    }
}