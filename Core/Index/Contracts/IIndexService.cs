namespace ConsoleApp2.Index.Contracts;

public interface IIndexService
{
    void WriteToIndex(IndexRecord item);
    void RemoveFromIndexByPath(string path);
    IndexRecord? GetRecordByPath(string path);
    List<IndexRecord>? GetRecordsByHash(string path);
}