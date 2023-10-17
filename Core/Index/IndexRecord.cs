namespace ConsoleApp2.Index;

public class IndexRecord
{
    public IndexRecord(string path, string hash, FileAttributes attributes)
    {
        Path = path;
        Hash = hash;
        Attributes = attributes;
    }

    public string Path { get; }
    public string Hash { get; }
    public FileAttributes Attributes { get; }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}