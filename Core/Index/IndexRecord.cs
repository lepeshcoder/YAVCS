namespace ConsoleApp2.Index;

public class IndexRecord
{
    public IndexRecord(string path, string hash, FileAttributes attributes)
    {
        Path = path;
        Hash = hash;
        Attributes = attributes;
    }

    public IndexRecord(string line)
    {
        var parts = line.Split(' ');
        Path = parts[0];
        Hash = parts[1];
        Attributes = FileAttributes.Normal;
    }

    public string Path { get; }
    public string Hash { get; }
    public FileAttributes Attributes { get; }

    public override string ToString()
    {
        return Path + ' ' + Hash + '\n';
    }
}