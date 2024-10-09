using System.Diagnostics.CodeAnalysis;
using WordleSolver.Playground.Toys;

namespace WordleSolver.Playground;

[ExcludeFromCodeCoverage]
public static class EntryPoint
{
    public static void Main()
    {
        var exerciser = new Excerciser();
        
        exerciser.RunAgainstAllWords();
    }
}