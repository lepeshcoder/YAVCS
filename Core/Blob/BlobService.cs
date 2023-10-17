using ConsoleApp2.Blob.Contracts;

namespace ConsoleApp2.Blob;

public class BlobService : IBlobService
{
    public void WriteBlob(byte[] data)
    {
        throw new NotImplementedException();
    }

    public void DeleteBlob(string hash)
    {
        throw new NotImplementedException();
    }

    public bool IsBlobExist(string hash)
    {
        throw new NotImplementedException();
    }
}