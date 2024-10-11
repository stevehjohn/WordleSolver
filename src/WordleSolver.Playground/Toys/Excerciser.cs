using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WordleSolver.Infrastructure;
using static WordleSolver.Common.Console;

namespace WordleSolver.Playground.Toys;

[ExcludeFromCodeCoverage]
public class Excerciser
{
    private const WordSet WordSet = Infrastructure.WordSet.Comprehensive;
    
    private readonly WordList _wordList = new(WordSet);

    private int _rounds;

    private int _totalSteps;

    private int _notFound;

    private int _overStepLimit;

    private int _minSteps = int.MaxValue;

    private int _maxSteps;
    
    private readonly Stack<Solver> _solvers = new();

    private readonly object _lock = new();

    public void RunAgainstAllWords()
    {
        var currentColour = ForegroundColor;
        
        OutputLine();
        OutputLine("  &Cyan;Playing all words!");
        OutputLine();

        var maxThreads = Environment.ProcessorCount - 1;

        var stopwatch = Stopwatch.StartNew();

        for (var i = 0; i < maxThreads; i++)
        {
            // ReSharper disable once InconsistentlySynchronizedField
            _solvers.Push(new Solver(WordSet));
        }

        Parallel.ForEach(
            _wordList.Words,
            new ParallelOptions { MaxDegreeOfParallelism = maxThreads },
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

        var fails = _notFound + _overStepLimit;
        
        OutputLine();
        OutputLine($"  &Cyan;Rounds Played&White;:      &Yellow;{_rounds}");
        OutputLine($"  &Cyan;Failures&White;:           &Yellow;{fails} ({(float) fails / _rounds * 100:N2}%)");
        OutputLine($"    &Cyan;Not Found&White;:        &Yellow;{_notFound}");
        OutputLine($"    &Cyan;Over Step Limit&White;:  &Yellow;{_overStepLimit}");
        OutputLine($"  &Cyan;Max Steps&White;:          &Yellow;{_maxSteps}");
        OutputLine($"  &Cyan;Min Steps&White;:          &Yellow;{_minSteps}");
        OutputLine($"  &Cyan;Mean Steps&White;:         &Yellow;{(float) _totalSteps / _rounds:N2}");
        OutputLine($"  &Cyan;Time Taken&White;:         &Yellow;{stopwatch.Elapsed.TotalMilliseconds:N2}ms");
        OutputLine();
        OutputLine("  &Cyan;Cheers&White;!");
        OutputLine();

        ForegroundColor = currentColour;
    }

    private void PlayGame(Solver solver, string expected)
    {
        var word = "SLANT";
        
        var steps = 0;
        
        solver.Reset();

        var builder = new StringBuilder();

        var failed = false;
        
        while (true)
        {
            steps++;
            
            var (result, nextWord, output) = PlayStep(solver, expected, word);

            builder.Append(output);

            if (result == StepResult.Failed)
            {
                failed = true;
                
                break;
            }

            if (result == StepResult.Solved)
            {
                break;
            }

            word = nextWord.ToUpper();
        }

        lock (_lock)
        {
            _rounds++;

            if (steps > _maxSteps)
            {
                _maxSteps = steps;
            }

            if (steps < _minSteps)
            {
                _minSteps = steps;
            }

            if (failed)
            {
                _notFound++;
            }

            if (steps > 6)
            {
                _overStepLimit++;
            }

            OutputLine(builder.ToString());

            _totalSteps += steps;

        }
    }

    private (StepResult Result, string NextWord, string output) PlayStep(Solver solver, string expected, string word)
    {
        expected = expected.ToUpper();

        word = word.ToUpper();

        var builder = new StringBuilder();

        builder.Append("  ");
        
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
            builder.Append($"  &Magenta;{expected}");
            
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