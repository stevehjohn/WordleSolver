using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WordleSolver.Extensions;
using WordleSolver.Infrastructure;
using static WordleSolver.Common.Console;

namespace WordleSolver.Playground.Toys;

[ExcludeFromCodeCoverage]
public class StartWordFinder
{
    private const WordSet WordSet = Infrastructure.WordSet.Scrabble;

    private readonly WordList _wordList = new(WordSet);

    private int _totalRounds;

    private int _lowestFails = int.MaxValue;

    private string _lowestFailsWord;

    private float _lowestMeanSteps = float.MaxValue;

    private string _lowestMeanStepsWord;

    private readonly Stack<Solver> _solvers = new();

    private readonly object _lock = new();
    
    public void FindBestStartWord()
    {
        ForegroundColor = ConsoleColor.Gray;
        
        OutputLine();
        OutputLine("  &Cyan;Playing all words&White;!");
        OutputLine();

        var stopwatch = Stopwatch.StartNew();

        var maxThreads = Environment.ProcessorCount / 4 * 3;
        
        for (var i = 0; i < maxThreads; i++)
        {
            _solvers.Push(new Solver(WordSet));
        }

        Parallel.ForEach(
            _wordList.Words,
            new ParallelOptions { MaxDegreeOfParallelism = maxThreads },
            startWord =>
            {
                var rounds = 0;

                var totalSteps = 0;

                var fails = 0;

                Solver solver;
                
                lock (_lock)
                {
                    solver = _solvers.Pop();
                }

                foreach (var expectedWord in _wordList.Words)
                {
                    PlayGame(solver, startWord.Word, expectedWord.Word, ref rounds, ref totalSteps, ref fails);
                }
                
                var meanSteps = (float) totalSteps / rounds;

                var isLowestMean = false;

                var isLowestFails = false;
                
                lock (_lock)
                {
                    _solvers.Push(solver);
                    
                    if (meanSteps < _lowestMeanSteps)
                    {
                        _lowestMeanSteps = meanSteps;

                        _lowestMeanStepsWord = startWord.Word;

                        isLowestMean = true;
                    }
                    
                    if (fails < _lowestFails)
                    {
                        _lowestFails = fails;

                        _lowestFailsWord = startWord.Word;

                        isLowestFails = true;
                    }

                    _totalRounds++;
                }
                
                var builder = new StringBuilder();

                builder.Append($"  &Cyan;{startWord.Word.ToUpper()}");

                builder.Append($"  &Green;Mean Steps&White;: {(isLowestMean ? "&Green;" : "&Gray;")}{meanSteps:N4}");

                builder.Append($"  &Yellow;Fails&White;: {(isLowestFails ? "&Yellow;" : "&Gray;")}{fails,5:N0}");

                builder.Append($"  &Green;{_lowestMeanStepsWord.ToUpper()}");

                builder.Append($"  &Yellow;{_lowestFailsWord.ToUpper()}");

                var remainingSeconds = (int) (stopwatch.Elapsed.TotalSeconds / _totalRounds * (_wordList.Words.Count - _totalRounds));

                var remaining = TimeSpan.FromSeconds(remainingSeconds);

                if (remaining.Days > 0)
                {
                    builder.Append($"  &Cyan;ETR&White;: {remaining.Days}d {remaining.Hours:D2}:{remaining.Minutes:D2}.{remaining.Seconds:D2} ({_totalRounds} / {_wordList.Words.Count})");
                }
                else
                {
                    builder.Append($"  &Cyan;ETR&White;: {remaining.Hours:D2}:{remaining.Minutes:D2}.{remaining.Seconds:D2} ({_totalRounds} / {_wordList.Words.Count})");
                }
                
                lock (_lock)
                {
                    OutputLine(builder.ToString());
                }
            });
        
        stopwatch.Stop();
        
        ForegroundColor = ConsoleColor.Cyan;
        
        OutputLine();
        OutputLine($"  &Cyan;Lowest mean steps&White;: &Green;{_lowestMeanStepsWord} &White;(&Green;{_lowestMeanSteps:N4}&White;)");
        OutputLine();
        OutputLine($"  &Cyan;Lowest failures&White;:   &Yellow;{_lowestFailsWord} &White;(&Yellow;{_lowestFails:N0}&White;)");
        OutputLine();
        OutputLine($"  &Cyan;Time Taken&White;:        &Cyan;{stopwatch.Elapsed.Hours:D2}:{stopwatch.Elapsed.Minutes:D2}.{stopwatch.Elapsed.Seconds:D2}");
        OutputLine();
        OutputLine("  &Cyan;Cheers&White;!");
        OutputLine();

        ForegroundColor = ConsoleColor.Green;
    }

    private static void PlayGame(Solver solver, string word, string expected, ref int rounds, ref int totalSteps, ref int fails)
    {
        var steps = 0;
        
        solver.Reset();
        
        while (true)
        {
            steps++;
            
            var (result, nextWord) = PlayStep(solver, expected, word);

            if (result == StepResult.Failed)
            {
                fails++;
                
                break;
            }

            if (result == StepResult.Solved)
            {
                break;
            }

            word = nextWord;
        }

        if (steps > 6)
        {
            fails++;
        }

        totalSteps += steps;

        rounds++;
    }

    private static (StepResult Result, string NextWord) PlayStep(Solver solver, string expected, string word)
    {
        for (var i = 0; i < 5; i++)
        {
            if (expected[i] == word[i])
            {
                solver.SetCorrect(expected[i], i);
                
                continue;
            }

            if (expected.ContainsCharacter(word[i]))
            {
                solver.AddIncorrect(word[i], i);
                
                continue;
            }
            
            solver.AddExcluded(word[i]);
        }

        var matches = solver.GetMatches();

        var match = matches.FirstOrDefault();

        if (match == null)
        {
            return (StepResult.Failed, null);
        }

        if (match.Equals(expected, StringComparison.InvariantCultureIgnoreCase))
        {
            return (StepResult.Solved, null);
        }

        return (StepResult.Continue, match);
    }
}