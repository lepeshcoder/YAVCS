namespace ConsoleApp2.FileSystem.Contracts;

public interface IFlieSystemProvider
{
   VcsRootDirectory? GetRootDirectory();

   bool IsVcsRootDirectory(string  dirPath);

}