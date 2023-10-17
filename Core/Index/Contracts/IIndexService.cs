namespace ConsoleApp2.Index.Contracts;

public interface IIndexService
{
    void WriteToIndex(IndexRecord item);
    void RemoveFromIndexByPath(string path);
    void RemoveFromIndexByHash(string hash);
    IndexRecord? GetRecordByPath(string path);
    bool IsRecordExist(string path);

    List<IndexRecord> GetRecordsByHash(string path);
}