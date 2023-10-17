using ConsoleApp2.FileSystem;
using ConsoleApp2.FileSystem.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp2;

public static class DependencyInjectionConfig
{
    public static IServiceProvider Configure()
    {
        return new ServiceCollection()
            .AddSingleton<IFlieSystemProvider,FileSystemProvider>()
            .BuildServiceProvider();
        
    }
}