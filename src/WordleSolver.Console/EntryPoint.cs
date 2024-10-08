using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Console;

public static class EntryPoint
{
    private static readonly WordList WordList = new(WordSet.Basic);
    
    public static void Main()
    {
        WriteLine();
        
        WriteLine("  Welcome to Wordle Solver.");

        while (true)
        {
            ExecuteRound();
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private static void ExecuteRound()
    {
        WriteLine($"{Environment.NewLine}  Enter any correctly placed letters separated by spaces:");
        
        Write("  > ");

        var correct = ReadLine();

        WriteLine($"{Environment.NewLine}  Enter any incorrectly placed letters:");
        
        Write("  > ");

        var incorrect = ReadLine();

        WriteLine($"{Environment.NewLine}  Enter any excluded letters:");
        
        Write("  > ");

        var excluded = ReadLine();
        
        WriteLine($"{Environment.NewLine}  Suggestions:");

        var result = WordList.GetMatches(correct, incorrect, excluded).Take(10);
        
        WriteLine($"{Environment.NewLine}  {string.Join(", ", result)}");
    }
}