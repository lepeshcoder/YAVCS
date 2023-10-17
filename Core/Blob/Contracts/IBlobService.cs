namespace ConsoleApp2.Blob.Contracts;

public interface IBlobService
{
    void WriteBlob(byte[] data);

    void DeleteBlob(string hash);
    
    bool IsBlobExist(string hash);
    
    
}