// See https://aka.ms/new-console-template for more information

using System.Net.Mime;
using System.Threading.Channels;
using ConsoleApp2;
using ConsoleApp2.Commands;
using ConsoleApp2.Commands.Contracts;
using ConsoleApp2.FileSystem;
using ConsoleApp2.FileSystem.Contracts;
using Microsoft.Extensions.DependencyInjection; 


var services = DependencyInjectionConfig.Configure();

var commands = new Dictionary<string, ICommand>
{
    { "init", new InitCommand(services.GetRequiredService<IFlieSystemProvider>()) }
};
try
{
    commands[args[0]].Execute(args.Skip(1).ToArray());
}
catch (Exception e)
{
    Console.WriteLine(e);
}

 