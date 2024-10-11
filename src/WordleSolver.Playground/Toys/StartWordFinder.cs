using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Playground.Toys;

[ExcludeFromCodeCoverage]
public class StartWordFinder
{
    private const int MaxThreads = 20;
    
    private const WordSet WordSet = Infrastructure.WordSet.Scrabble;

    private readonly WordList _wordList = new(WordSet);

    private int _totalRounds;
    
    private int _rounds;

    private int _totalSteps;

    private int _fails;

    private int _lowestFails = int.MaxValue;

    private string _lowestFailsWord;

    private float _lowestMeanSteps = float.MaxValue;

    private string _lowestMeanStepsWord;

    private readonly Stack<Solver> _solvers = new();

    private readonly object _lock = new();
    
    public void FindBestStartWord()
    {
        ForegroundColor = ConsoleColor.Gray;
        
        WriteLine();
        WriteLine("  Playing all words!");
        WriteLine();

        var stopwatch = Stopwatch.StartNew();

        for (var i = 0; i < MaxThreads; i++)
        {
            _solvers.Push(new Solver(WordSet));
        }

        Parallel.ForEach(
            _wordList.Words,
            new ParallelOptions { MaxDegreeOfParallelism = MaxThreads },
            startWord =>
            {
                _rounds = 0;

                _totalSteps = 0;

                _fails = 0;

                Solver solver;
                
                lock (_lock)
                {
                    solver = _solvers.Pop();
                }

                foreach (var expectedWord in _wordList.Words)
                {
                    PlayGame(solver, startWord, expectedWord);
                }

                lock (_lock)
                {
                    _solvers.Push(solver);
                }

                lock (_lock)
                {
                    Write($"  {startWord}");

                    var meanSteps = (float) _totalSteps / _rounds;

                    ForegroundColor = ConsoleColor.Green;

                    Write("  Mean Steps: ");

                    ForegroundColor = ConsoleColor.Gray;

                    if (meanSteps < _lowestMeanSteps)
                    {
                        _lowestMeanSteps = meanSteps;

                        _lowestMeanStepsWord = startWord;

                        ForegroundColor = ConsoleColor.Green;
                    }

                    Write($"{meanSteps:N4}  ");

                    ForegroundColor = ConsoleColor.Yellow;

                    Write("Fails: ");

                    ForegroundColor = ConsoleColor.Gray;

                    if (_fails < _lowestFails)
                    {
                        _lowestFails = _fails;

                        _lowestFailsWord = startWord;

                        ForegroundColor = ConsoleColor.Yellow;
                    }

                    Write($"{_fails,5:N0}");

                    ForegroundColor = ConsoleColor.Gray;

                    _totalRounds++;

                    ForegroundColor = ConsoleColor.Green;

                    Write($"  {_lowestMeanStepsWord}");

                    ForegroundColor = ConsoleColor.Yellow;

                    Write($"  {_lowestFailsWord}");

                    ForegroundColor = ConsoleColor.Gray;

                    var remainingSeconds = (int) (stopwatch.Elapsed.TotalSeconds / _totalRounds * (_wordList.Words.Count - _totalRounds));

                    var remaining = TimeSpan.FromSeconds(remainingSeconds);

                    if (remaining.Days > 0)
                    {
                        Write($"  ETR: {remaining.Days}d {remaining.Hours:D2}:{remaining.Minutes:D2}.{remaining.Seconds:D2} ({_totalRounds} / {_wordList.Words.Count})");
                    }
                    else
                    {
                        Write($"  ETR: {remaining.Hours:D2}:{remaining.Minutes:D2}.{remaining.Seconds:D2} ({_totalRounds} / {_wordList.Words.Count})");
                    }

                    WriteLine();
                }
            });
        
        stopwatch.Stop();
        
        ForegroundColor = ConsoleColor.Cyan;
        
        WriteLine();
        WriteLine($"  Lowest failures:   {_lowestFailsWord} ({_lowestFails:N0})");
        WriteLine();
        WriteLine($"  Lowest mean steps: {_lowestMeanStepsWord} ({_lowestMeanSteps:N4})");
        WriteLine();
        WriteLine($"  Time Taken:        {stopwatch.Elapsed.Hours:D2}:{stopwatch.Elapsed.Minutes:D2}.{stopwatch.Elapsed.Seconds:D2}");
        WriteLine();
        WriteLine("  Cheers!");
        WriteLine();

        ForegroundColor = ConsoleColor.Green;
    }

    private void PlayGame(Solver solver, string word, string expected)
    {
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

        if (steps > 6)
        {
            _fails++;
        }

        _totalSteps += steps;

        _rounds++;
    }

    private static (StepResult Result, string NextWord) PlayStep(Solver solver, string expected, string word)
    {
        expected = expected.ToUpper();

        word = word.ToUpper();
        
        for (var i = 0; i < 5; i++)
        {
            if (expected[i] == word[i])
            {
                solver.SetCorrect(expected[i], i);
                
                continue;
            }

            if (expected.Contains(word[i]))
            {
                solver.AddIncorrect(word[i], i);
                
                continue;
            }
            
            solver.AddExcluded(word[i]);
        }

        var matches = solver.GetMatches();

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