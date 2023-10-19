using ConsoleApp2.Blob;
using ConsoleApp2.Blob.Contracts;
using ConsoleApp2.FileSystem;
using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.FileSystem.Services;
using ConsoleApp2.Hash;
using ConsoleApp2.Hash.Contracts;
using ConsoleApp2.Index.Contracts;
using ConsoleApp2.Index.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp2;

public static class DependencyInjectionConfig
{
    public static IServiceProvider Configure()
    {
        return new ServiceCollection()
            .AddSingleton<IFileSystemProvider,FileSystemProvider>()
            .AddSingleton<IHashService,HashService>()
            .AddSingleton<IBlobService,BlobService>()
            .AddSingleton<IIndexService,IndexService>()
            .BuildServiceProvider();
    }
}