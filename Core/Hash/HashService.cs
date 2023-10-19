using System.Security.Cryptography;
using ConsoleApp2.Hash.Contracts;

namespace ConsoleApp2.Hash;

public class HashService : IHashService
{
    public string GetHash(byte[] data)
    {
        var hashBytes = SHA256.HashData(data);
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return hashString;
    }
}