using System.Diagnostics;
using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Playground.Toys;

public class StartWordFinder
{
    private readonly WordList _wordList = new(WordSet.Basic);

    private readonly Solver _solver = new(WordSet.Basic);

    private int _totalRounds;
    
    private int _rounds;

    private int _totalSteps;

    private int _fails;

    private int _lowestFails = int.MaxValue;

    private string _lowestFailsWord;

    private float _lowestMeanSteps = float.MaxValue;

    private string _lowestMeanStepsWord;
    
    public void FindBestStartWord()
    {
        ForegroundColor = ConsoleColor.Cyan;
        
        WriteLine();
        WriteLine("  Playing all words!");
        WriteLine();

        var stopwatch = Stopwatch.StartNew();

        foreach (var startWord in _wordList.Words.Take(20))
        {
            _rounds = 0;

            _totalSteps = 0;

            _fails = 0;
            
            Write($"  {startWord}");
            
            foreach (var expectedWord in _wordList.Words)
            {
                PlayGame(startWord, expectedWord);
            }

            var meanSteps = (float) _totalSteps / _rounds;
            
            Write($"  Fails: {_fails,3:N0}    Mean Steps: {meanSteps:N2}");

            if (meanSteps < _lowestMeanSteps)
            {
                _lowestMeanSteps = meanSteps;

                _lowestMeanStepsWord = startWord;
                
                Write("  New lowest mean steps.");
            }

            if (_fails < _lowestFails)
            {
                _lowestFails = _fails;

                _lowestFailsWord = startWord;
                
                Write("  New lowest fails.");
            }

            _totalRounds++;

            if (_totalRounds > 10)
            {
                var remainingSeconds = (int) stopwatch.Elapsed.TotalSeconds / _totalRounds * (_wordList.Words.Count - _totalRounds);

                var remaining = TimeSpan.FromSeconds(remainingSeconds);
                
                Write($"  ETR: {remaining.Hours:D2}:{remaining.Minutes}.{remaining.Seconds}");
            }

            WriteLine();
        }
        
        stopwatch.Stop();
        
        ForegroundColor = ConsoleColor.Cyan;
        
        WriteLine();
        WriteLine($"  Lowest failures:   {_lowestFailsWord}");
        WriteLine();
        WriteLine($"  Lowest mean steps: {_lowestMeanStepsWord}");
        WriteLine();
        WriteLine($"  Time Taken:        {stopwatch.Elapsed.Hours:D2}:{stopwatch.Elapsed.Minutes}.{stopwatch.Elapsed.Seconds}");
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