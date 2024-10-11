using System.Diagnostics.CodeAnalysis;
using WordleSolver.Playground.Toys;

namespace WordleSolver.Playground;

[ExcludeFromCodeCoverage]
public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        if (arguments.Length > 0 && arguments[0].Equals("finder", StringComparison.InvariantCultureIgnoreCase))
        {
            var finder = new StartWordFinder();
        
            finder.FindBestStartWord();
        }
        else
        {
            var exerciser = new Excerciser();
            
            exerciser.RunAgainstAllWords();
        }
    }
}