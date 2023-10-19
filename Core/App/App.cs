using ConsoleApp2.Blob.Contracts;
using ConsoleApp2.Commands;
using ConsoleApp2.Commands.Contracts;
using ConsoleApp2.FileSystem.Contracts;
using ConsoleApp2.Hash.Contracts;
using ConsoleApp2.Index.Contracts;
using ConsoleApp2.Index.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp2.App;

public static class App
{

    private static Dictionary<string, ICommand> _commands = new();
    private static readonly IServiceProvider Services = DependencyInjectionConfig.Configure();
    
    public static void Configure()
    {
        try
        {
            Services.GetRequiredService<IIndexService>().Initialize();
            _commands = new Dictionary<string, ICommand>
            {
                { 
                    "init", 
                    new InitCommand(Services.GetRequiredService<IFileSystemProvider>())
                },
                {
                    "add",
                    new AddCommand(
                        Services.GetRequiredService<IFileSystemProvider>(),
                        Services.GetRequiredService<IIndexService>(),
                        Services.GetRequiredService<IBlobService>(),
                        Services.GetRequiredService<IHashService>())
                },
                {
                    "status",
                    new StatusCommand(
                        Services.GetRequiredService<IIndexService>(),
                        Services.GetRequiredService<IFileSystemProvider>(),
                        Services.GetRequiredService<IHashService>())
                },
                {
                    "unstage",
                    new UnStageCommand(
                        Services.GetRequiredService<IFileSystemProvider>(),
                        Services.GetRequiredService<IIndexService>(),
                        Services.GetRequiredService<IBlobService>())
                }
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
      
    }

    public static void Run(string[] args)
    {
        try
        {
            _commands[args[0]].Execute(args.Skip(1).ToArray());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}