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
        expected = "ABCED";
        var steps = 0;
        
        _solver.Reset();
        
        while (true)
        {
            steps++;
            
            var result = PlayStep(expected, word);

            if (result == StepResult.Failed)
            {
                _fails++;
                
                break;
            }

            if (result == StepResult.Solved)
            {
                break;
            }
        }

        _totalSteps += steps;

        _rounds++;
    }

    private StepResult PlayStep(string expected, string word)
    {
        Write("  ");
        
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
        
        WriteLine();

        ForegroundColor = ConsoleColor.Green;

        return StepResult.Solved;
    }
}