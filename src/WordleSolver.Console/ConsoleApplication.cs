using System.Diagnostics.CodeAnalysis;
using System.Text;
using WordleSolver.Infrastructure;
using static WordleSolver.Common.Console;

namespace WordleSolver.Console;

[ExcludeFromCodeCoverage]
public class ConsoleApplication
{
    private readonly Solver _solver = new(WordSet.Scrabble);

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
        
        OutputLine("  &Cyan;Select&White;:");
        
        OutputLine();
        
        OutputLine("  &Yellow;1&White;: &Cyan;Reset game&White;.");
        OutputLine("  &Yellow;2&White;: &Cyan;Quit&White;.");
        
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
                OutputLine();
                OutputLine("  &Cyan;Thank you for using Wordle Solver&White;. &Cyan;Bye&White;.");
                OutputLine();
                ForegroundColor = _previousColour;

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

        var result = _solver.GetMatches().Select(FormatSuggestion).Take(20).ToList();

        if (result.Count > 0)
        {
            Output("  ");
        
            for (var i = 0; i < 20; i++)
            {
                if (i >= result.Count)
                {
                    break;
                }

                Output(result[i]);

                if (i < result.Count - 1)
                {
                    Output("&White;,");
                }

                if (i == 9)
                {
                    OutputLine();
                
                    Output("  ");
                }
            }
            
            OutputLine();
        }
        else
        {
            OutputLine("  &Magenta;None&White;.");
        }


        return result.Count > 1;
    }

    private string FormatSuggestion(string word)
    {
        var builder = new StringBuilder();
        
        for (var i = 0; i < word.Length; i++)
        {
            if (_solver.Correct.Any(t => t.Character == word[i] && t.Position == i))
            {
                builder.Append($"&Green;{word[i]}");
                
                continue;
            }

            if (_solver.Incorrect.Any(t => t.Character == word[i]))
            {
                builder.Append($"&Yellow;{word[i]}");
                
                continue;
            }

            builder.Append($"&Gray;{word[i]}");
        }

        return builder.ToString();
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