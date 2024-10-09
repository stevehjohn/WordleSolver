using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Playground.Toys;

[ExcludeFromCodeCoverage]
public class Excerciser
{
    private readonly WordList _wordList = new(WordSet.OriginalAllowedAnswers);

    private readonly Solver _solver = new(WordSet.OriginalAllowedAnswers);

    private int _rounds;

    private int _totalSteps;

    private int _fails;

    private int _minSteps = int.MaxValue;

    private int _maxSteps;
    
    public void RunAgainstAllWords()
    {
        ForegroundColor = ConsoleColor.Cyan;
        
        WriteLine();
        WriteLine("  Playing all words!");
        WriteLine();

        var stopwatch = Stopwatch.StartNew();
        
        foreach (var word in _wordList.Words)
        {
            PlayGame(word);
        }
        
        stopwatch.Stop();
        
        ForegroundColor = ConsoleColor.Cyan;
        
        WriteLine();
        WriteLine($"  Rounds Played: {_rounds}");
        WriteLine($"  Failures:      {_fails} ({(float) _fails / _rounds:N2}%)");
        WriteLine($"  Max Steps:     {_maxSteps}");
        WriteLine($"  Min Steps:     {_minSteps}");
        WriteLine($"  Mean Steps:    {(float) _totalSteps / _rounds:N2}");
        WriteLine($"  Time Taken:    {stopwatch.Elapsed.TotalMilliseconds:N2}ms");
        WriteLine();
        WriteLine("  Cheers!");
        WriteLine();

        ForegroundColor = ConsoleColor.Green;
    }

    private void PlayGame(string expected)
    {
        var word = "SLANT";
        
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

            word = nextWord.ToUpper();
        }

        if (steps > _maxSteps)
        {
            _maxSteps = steps;
        }

        if (steps < _minSteps)
        {
            _minSteps = steps;
        }

        if (steps > 6)
        {
            _fails++;
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
            ForegroundColor = ConsoleColor.Magenta;
            
            WriteLine($"  {expected}");
            
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