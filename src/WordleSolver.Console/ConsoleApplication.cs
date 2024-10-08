using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Console;

public class ConsoleApplication
{
    private readonly Solver _solver = new(WordSet.Basic);
    
    public void Run()
    {
        ShowWelcome();

        while (true)
        {
            while (true)
            {
                if (! ExecuteRound())
                {
                    break;
                }
            }
            
            // Do menu here
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private bool ExecuteRound()
    {
        OutputLine("Enter correctly placed letters:");
        
        Output("> ");

        var input = ReadLine();

        if (input == "*")
        {
            return false;
        }

        OutputLine("Enter incorrectly placed letters:");
        
        Output("> ");

        input = ReadLine();

        if (input == "*")
        {
            return false;
        }

        OutputLine("Enter any excluded letters:");
        
        Output("> ");

        input = ReadLine();

        if (input == "*")
        {
            return false;
        }
        
        OutputLine("Suggestions:");

        var result = _solver.GetMatches().Take(10);
        
        OutputLine($"{string.Join(", ", result)}");

        return true;
    }

    private static void ShowWelcome()
    {
        OutputLine("Welcome to Wordle Solver!");
        
        OutputLine("When entering correctly or incorrectly placed letters,");
        
        OutputLine("separate with hyphens to indicate the position.", false);

        OutputLine("E.g. at the prompt:");
        
        OutputLine("> -A--E");
        
        OutputLine("Excluded letters are not position sensitive.");
        
        OutputLine("Enter * for options.");
        
        OutputLine(string.Empty, false);
    }

    private static void OutputLine(string text = null, bool newLineBefore = true)
    {
        if (newLineBefore)
        {
            WriteLine();
        }
        
        if (string.IsNullOrWhiteSpace(text))
        {
            WriteLine();
        }
        else
        {
            WriteLine($"  {text}");
        }
    }

    private static void Output(string text)
    {
        Write($"  {text}");
    }
}