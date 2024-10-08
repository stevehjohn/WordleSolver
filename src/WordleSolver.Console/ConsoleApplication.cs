using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Console;

public class ConsoleApplication
{
    private readonly Solver _solver = new(WordSet.Basic);

    private ConsoleColor _previousColour;
    
    public void Run()
    {
        _previousColour = ForegroundColor;
        
        ShowWelcome();

        while (true)
        {
            while (true)
            {
                if (! ExecuteRound())
                {
                    break;
                }
                
                WriteLine();
                
                Write("  ");

                _solver.DumpState();
                
                WriteLine();
            }
            
            Menu();
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private void Menu()
    {
        OutputLine("Select:");
        
        OutputLine("1: Reset game.", false);
        OutputLine("2: Quit.", false);
        
        Output("> ");

        var input = ReadLine();

        switch (input)
        {
            case "1":
                _solver.Reset();
                break;
            
            case "2":
                OutputLine("Thank you for using Wordle Solver. Bye.");
                OutputLine();
                ForegroundColor = _previousColour;
                Environment.Exit(0);
                
                break;
        }
    }

    private bool ExecuteRound()
    {
        OutputLine("Enter correctly placed letters:");
        
        Output("> ");

        var input = ReadLine() ?? string.Empty;

        if (input == "*")
        {
            return false;
        }

        for (var i = 0; i < input.Length; i++)
        {
            if (! char.IsLetter(input[i]))
            {
                continue;
            }

            _solver.SetCorrect(input[i], i);
        }

        OutputLine("Enter incorrectly placed letters:");
        
        Output("> ");

        input = ReadLine() ?? string.Empty;

        if (input == "*")
        {
            return false;
        }
        
        for (var i = 0; i < input.Length; i++)
        {
            if (! char.IsLetter(input[i]))
            {
                continue;
            }

            _solver.AddIncorrect(input[i], i);
        }

        OutputLine("Enter any excluded letters:");
        
        Output("> ");

        input = ReadLine() ?? string.Empty;

        if (input == "*")
        {
            return false;
        }
                
        for (var i = 0; i < input.Length; i++)
        {
            _solver.AddExcluded(input[i]);
        }
        
        OutputLine("Suggestions:");

        var result = _solver.GetMatches().Take(10);
        
        OutputLine($"{string.Join(", ", result)}");

        return true;
    }

    private static void ShowWelcome()
    {
        ForegroundColor = ConsoleColor.Cyan;
        
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