using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Playground.Toys;

[ExcludeFromCodeCoverage]
public class Excerciser
{
    private const int MaxThreads = 20;
    
    private readonly WordList _wordList = new(WordSet.Scrabble);

    private int _rounds;

    private int _totalSteps;

    private int _fails;

    private int _minSteps = int.MaxValue;

    private int _maxSteps;
    
    private readonly Stack<Solver> _solvers = new();

    private readonly object _lock = new();

    public void RunAgainstAllWords()
    {
        ForegroundColor = ConsoleColor.Cyan;
        
        WriteLine();
        WriteLine("  Playing all words!");
        WriteLine();

        var stopwatch = Stopwatch.StartNew();

        for (var i = 0; i < MaxThreads; i++)
        {
            // ReSharper disable once InconsistentlySynchronizedField
            _solvers.Push(new Solver(WordSet.Scrabble));
        }

        Parallel.ForEach(
            _wordList.Words,
            new ParallelOptions { MaxDegreeOfParallelism = 20 },
            word =>
            {
                
                Solver solver;
                
                lock (_lock)
                {
                    solver = _solvers.Pop();
                }

                PlayGame(solver, word);
                
                lock (_lock)
                {
                    _solvers.Push(solver);
                }
            });
        
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

    private void PlayGame(Solver solver, string expected)
    {
        var word = "SLANT";
        
        var steps = 0;
        
        solver.Reset();
        
        while (true)
        {
            steps++;
            
            var (result, nextWord) = PlayStep(solver, expected, word);

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

    private (StepResult Result, string NextWord, string output) PlayStep(Solver solver, string expected, string word)
    {
        Write("  ");

        expected = expected.ToUpper();

        word = word.ToUpper();

        var builder = new StringBuilder();
        
        for (var i = 0; i < 5; i++)
        {
            if (expected[i] == word[i])
            {
                builder.Append($"&Green;{expected[i]}");
                
                solver.SetCorrect(expected[i], i);
                
                continue;
            }

            if (expected.Contains(word[i]))
            {
                builder.Append($"&Yellow;{word[i]}");
                
                solver.AddIncorrect(word[i], i);
                
                continue;
            }

            builder.Append($"&Gray;{word[i]}");
            
            solver.AddExcluded(word[i]);
        }

        var matches = solver.GetMatches();

        if (matches.Count == 0)
        {
            builder.Append($"  &Magents;{expected}");
            
            return (StepResult.Failed, null, builder.ToString());
        }

        if (matches.First().Equals(expected, StringComparison.InvariantCultureIgnoreCase))
        {
            builder.Append($"  &Green;{expected}");
            
            return (StepResult.Solved, null, builder.ToString());
        }

        return (StepResult.Continue, matches.First(), builder.ToString());
    }
}