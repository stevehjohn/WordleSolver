using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Playground.Toys;

public class Excerciser
{
    private readonly WordList _wordList = new(WordSet.Basic);

    private readonly Solver _solver = new(WordSet.Basic);

    private int _rounds;

    private int _totalSteps;

    private int _fails;
    
    public void RunAgainstAllWords()
    {
        ForegroundColor = ConsoleColor.Cyan;
        
        WriteLine();
        WriteLine("  Playing all words!");
        WriteLine();
        
        foreach (var word in _wordList.Words)
        {
            PlayGame(word);
            
            break;
        }
        
        WriteLine();
        WriteLine($"  Rounds Played: {_rounds}");
        WriteLine($"  Average Steps: {(float) _totalSteps / _rounds:F2}");
        WriteLine($"  Failures:      {_fails}");
        WriteLine();
        WriteLine("  Cheers!");
        WriteLine();

        ForegroundColor = ConsoleColor.Green;
    }

    private void PlayGame(string expected)
    {
        var word = "AUDIO";
        
        var steps = 0;
        
        _solver.Reset();
        
        while (true)
        {
            steps++;
            
            var (result, nextWord) = PlayStep(expected, word);

            if (result == StepResult.Failed)
            {
                _fails++;
                
                break;
            }

            if (result == StepResult.Solved)
            {
                break;
            }

            word = nextWord; //.ToUpper();
        }

        _totalSteps += steps;

        _rounds++;
    }

    private (StepResult Result, string NextWord) PlayStep(string expected, string word)
    {
        Write("  ");

        expected = expected.ToUpper();

        word = word.ToUpper();
        
        for (var i = 0; i < 5; i++)
        {
            if (expected[i] == word[i])
            {
                ForegroundColor = ConsoleColor.Green;
                
                Write(expected[i]);
                
                _solver.SetCorrect(expected[i], i);
                
                continue;
            }

            if (expected.Contains(word[i]))
            {
                ForegroundColor = ConsoleColor.Yellow;
                
                Write(word[i]);
                
                _solver.AddIncorrect(word[i], i);
                
                continue;
            }

            ForegroundColor = ConsoleColor.Gray;
                
            Write(word[i]);
            
            _solver.AddExcluded(word[i]);
        }

        ForegroundColor = ConsoleColor.Green;

        var matches = _solver.GetMatches();

        if (matches.Count == 0)
        {
            return (StepResult.Failed, null);
        }

        if (matches.First().Equals(expected, StringComparison.InvariantCultureIgnoreCase))
        {
            ForegroundColor = ConsoleColor.Green;
            
            WriteLine($"  {expected}");
            
            return (StepResult.Solved, null);
        }

        return (StepResult.Continue, matches.First());
    }
}