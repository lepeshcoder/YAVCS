using ConsoleApp2.Index.Contracts;

namespace ConsoleApp2.Index.Services;

public class IndexService : IIndexService
{
    public void WriteToIndex(IndexRecord item)
    {
        throw new NotImplementedException();
    }

    public void RemoveFromIndexByPath(string path)
    {
        throw new NotImplementedException();
    }

    public void RemoveFromIndexByHash(string hash)
    {
        throw new NotImplementedException();
    }

    public IndexRecord? GetRecordByPath(string path)
    {
        throw new NotImplementedException();
    }

    public bool IsRecordExist(string path)
    {
        throw new NotImplementedException();
    }

    public List<IndexRecord> GetRecordsByHash(string path)
    {
        throw new NotImplementedException();
    }

    public List<IndexRecord> GetHashByPath(string path)
    {
        throw new NotImplementedException();
    }
}