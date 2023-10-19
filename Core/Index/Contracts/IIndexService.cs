using System.Net.Http.Json;

namespace ConsoleApp2.Index.Contracts;

public interface IIndexService
{
    Dictionary<string,IndexRecord> RecordsByPath { get; }
    
    void WriteToIndex(IndexRecord item);
    void RemoveFromIndexByPath(string path);
    IndexRecord? GetRecordByPath(string path);
    List<IndexRecord>? GetRecordsByHash(string path);
}