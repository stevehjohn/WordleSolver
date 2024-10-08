using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Console;

public static class EntryPoint
{
    private static WordList _wordList = new(WordSet.Basic);
    
    public static void Main()
    {
        WriteLine();
        
        WriteLine("Welcome to Wordle Solver.");

        while (true)
        {
            ExecuteRound();
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private static void ExecuteRound()
    {
    }
}