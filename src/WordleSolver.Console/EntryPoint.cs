using System.Diagnostics.CodeAnalysis;

namespace WordleSolver.Console;

[ExcludeFromCodeCoverage]
public static class EntryPoint
{
    public static void Main()
    {
        var application = new ConsoleApplication();

        application.Run();
    }
}