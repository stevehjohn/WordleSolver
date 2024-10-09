using System.Diagnostics;
using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Playground.Toys;

public class StartWordFinder
{
    private readonly WordList _wordList = new(WordSet.Basic);

    private readonly Solver _solver = new(WordSet.Basic);

    private int _rounds;

    private int _totalSteps;

    private int _fails;

    private int _minSteps = int.MaxValue;

    private int _maxSteps;
    
    public void FindBestStartWord()
    {
        ForegroundColor = ConsoleColor.Cyan;
        
        WriteLine();
        WriteLine("  Playing all words!");
        WriteLine();

        var stopwatch = Stopwatch.StartNew();

        foreach (var startWord in _wordList.Words)
        {
            WriteLine($"  {startWord}");
            
            foreach (var expectedWord in _wordList.Words)
            {
                PlayGame(startWord, expectedWord);
            }
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

    private void PlayGame(string word, string expected)
    {
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
        expected = expected.ToUpper();

        word = word.ToUpper();
        
        for (var i = 0; i < 5; i++)
        {
            if (expected[i] == word[i])
            {
                _solver.SetCorrect(expected[i], i);
                
                continue;
            }

            if (expected.Contains(word[i]))
            {
                _solver.AddIncorrect(word[i], i);
                
                continue;
            }
            
            _solver.AddExcluded(word[i]);
        }

        var matches = _solver.GetMatches();

        if (matches.Count == 0)
        {
            return (StepResult.Failed, null);
        }

        if (matches.First().Equals(expected, StringComparison.InvariantCultureIgnoreCase))
        {
            return (StepResult.Solved, null);
        }

        return (StepResult.Continue, matches.First());
    }
}