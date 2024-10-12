using System.Diagnostics.CodeAnalysis;
using WordleSolver.Infrastructure;
using static WordleSolver.Common.Console;

namespace WordleSolver.Console;

[ExcludeFromCodeCoverage]
public class ConsoleApplication
{
    private readonly Solver _solver = new(WordSet.OriginalAllowedAnswers);

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
                
                OutputLine();
                
                Output("  ");

                _solver.DumpState();
                
                OutputLine();
            }

            if (Menu())
            {
                break;
            }
        }
    }

    private bool Menu()
    {
        OutputLine();
        
        OutputLine("  Select:");
        
        OutputLine();
        
        OutputLine("  1: Reset game.");
        OutputLine("  2: Quit.");
        
        OutputLine();
        
        Output("  > ");

        var input = ReadLine();

        switch (input)
        {
            case "1":
                Clear();
                _solver.Reset();

                return false;
            
            case "2":
                OutputLine("  Thank you for using Wordle Solver. Bye.");
                OutputLine();
                ForegroundColor = _previousColour;
                Environment.Exit(0);

                return true;
        }

        return false;
    }

    private bool ExecuteRound()
    {
        OutputLine();
        
        OutputLine("  Enter correctly placed letters:");
        
        Output("  > ");

        var input = ReadLine();

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

        OutputLine("  Enter incorrectly placed letters:");
        
        Output("  > ");

        input = ReadLine();

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

        OutputLine("  Enter any excluded letters:");
        
        Output("  > ");

        input = ReadLine();

        if (input == "*")
        {
            return false;
        }
                
        for (var i = 0; i < input.Length; i++)
        {
            _solver.AddExcluded(input[i]);
        }
                        
        Clear();

        OutputLine();
        
        OutputLine("  Suggestions:");

        var result = _solver.GetMatches().Take(10).ToList();
        
        OutputLine($"  {string.Join(", ", result)}");

        return result.Count > 1;
    }

    private static void ShowWelcome()
    {
        ForegroundColor = ConsoleColor.Cyan;
        
        OutputLine();
        
        OutputLine("  Welcome to Wordle Solver!");
        
        OutputLine();
        
        OutputLine("  When entering correctly or incorrectly placed letters,");
        
        OutputLine("  separate with hyphens to indicate the position.");

        OutputLine();
        
        OutputLine("  E.g. at the prompt:");

        OutputLine();
        
        OutputLine("  > -A--E");
        
        OutputLine();
        
        OutputLine("  Excluded letters are not position sensitive.");
        
        OutputLine();
        
        OutputLine("  Enter * for options.");
    }
}