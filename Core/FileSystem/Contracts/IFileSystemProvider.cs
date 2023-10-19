namespace ConsoleApp2.FileSystem.Contracts;

public interface IFileSystemProvider
{
   VcsRootDirectory? GetRootDirectory();

   bool IsVcsRootDirectory(string  dirPath);

}