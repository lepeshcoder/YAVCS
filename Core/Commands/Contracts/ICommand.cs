namespace ConsoleApp2.Commands.Contracts;

public interface ICommand
{
    string Description { get; }
    
    void Execute(string[] args);
}