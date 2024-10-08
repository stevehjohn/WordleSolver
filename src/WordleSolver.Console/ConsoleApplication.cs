using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Console;

public class ConsoleApplication
{
    private readonly WordList _wordList = new(WordSet.Basic);
    
    public void Run()
    {
        ShowWelcome();

        while (true)
        {
            ExecuteRound();
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private void ExecuteRound()
    {
        OutputLine("Enter any correctly placed letters separated by spaces or dots:");
        
        Output("> ");

        var correct = ReadLine();

        OutputLine("Enter any incorrectly placed letters:");
        
        Output("> ");

        var incorrect = ReadLine();

        OutputLine("Enter any excluded letters:");
        
        Output("> ");

        var excluded = ReadLine();
        
        OutputLine("Suggestions:");

        var result = _wordList.GetMatches(correct, incorrect, excluded).Take(10);
        
        OutputLine($"{string.Join(", ", result)}");
    }

    private static void ShowWelcome()
    {
        OutputLine("Welcome to Wordle Solver!");
        
        OutputLine("When entering correctly or placed letters,");
        
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