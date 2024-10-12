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
        
        OutputLine("  &Cyan;Enter correctly placed letters&White;:");
        
        OutputLine();

        Output("  &Yellow;> &Green;");

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

        OutputLine();
        
        OutputLine("  &Cyan;Enter incorrectly placed letters&White;:");
        
        OutputLine();

        Output("  &Yellow;> ");

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

        OutputLine();

        OutputLine("  &Cyan;Enter any excluded letters&White;:");
        
        OutputLine();

        Output("  &Yellow;> &Gray;");

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
        
        OutputLine("  &Cyan;Suggestions&White;:");

        OutputLine();

        var result = _solver.GetMatches().Take(10).ToList();
        
        OutputLine($"  {string.Join("&White;, ", result)}");

        return result.Count > 1;
    }

    private static void ShowWelcome()
    {
        ForegroundColor = ConsoleColor.Cyan;
        
        OutputLine();
        
        OutputLine("  &Cyan;Welcome to Wordle Solver&White;!");
        
        OutputLine();
        
        OutputLine("  &Cyan;When entering correctly or incorrectly placed letters&White;,");
        
        OutputLine("  &Cyan;separate with hyphens to indicate the position&White;.");

        OutputLine();
        
        OutputLine("  &Cyan;E&White;.&Cyan;g&White;.&Cyan; at the prompt&White;:");

        OutputLine();
        
        OutputLine("  &Yellow;> &Green;-A--E");
        
        OutputLine();
        
        OutputLine("  &Cyan;Excluded letters are not position sensitive&White;.");
        
        OutputLine();
        
        OutputLine("  &Cyan;Enter &White;*&Cyan; for options&White;.");
    }
}