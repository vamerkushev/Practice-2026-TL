namespace Fighters.SystemConsole;

public class SystemConsole : ISystemConsole
{
    public void WriteLine( string message ) => Console.WriteLine( message );
    public string? ReadLine() => Console.ReadLine();
}