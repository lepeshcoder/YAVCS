namespace ConsoleApp2.Hash.Contracts;

public interface IHashService
{
    string GetHash(byte[] data);
}