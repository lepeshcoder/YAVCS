using ConsoleApp2.Blob.Contracts;
using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.Hash.Contracts;

namespace ConsoleApp2.Blob;

public class BlobService : IBlobService
{
    private readonly IHashService _hashService;
    private readonly IFileSystemProvider _fileSystemProvider;

    public BlobService(IHashService hashService, IFileSystemProvider fileSystemProvider)
    {
        _hashService = hashService;
        _fileSystemProvider = fileSystemProvider;
    }

    public void WriteBlob(byte[] data)
    {
        var hash = _hashService.GetHash(data);
        var blobPath = _fileSystemProvider.GetRootDirectory()!.BlobsDirectory + '/' + hash; 
        File.WriteAllBytes(blobPath,data);
    }

    public void DeleteBlob(string hash)
    {
        var blobPath =_fileSystemProvider.GetRootDirectory()!.BlobsDirectory + '/' + hash; 
        File.Delete(blobPath);
    }

    public bool IsBlobExist(string hash)
    {
        var blobPath = _fileSystemProvider.GetRootDirectory()!.BlobsDirectory + '/' + hash;
        return File.Exists(blobPath);
    }
}