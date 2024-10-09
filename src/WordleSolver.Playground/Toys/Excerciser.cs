using WordleSolver.Infrastructure;
using static System.Console;

namespace WordleSolver.Playground.Toys;

public class Excerciser
{
    private readonly WordList _wordList = new(WordSet.Basic);

    private readonly Solver _solver = new Solver(WordSet.Basic);

    private int _rounds = 0;

    private int _totalSteps = 0;

    private int _fails = 0;
    
    public void RunAgainstAllWords()
    {
        WriteLine();
        WriteLine("  Playing all words!");
        
        foreach (var word in _wordList.Words)
        {
            PlayGame(word);
        }
        
        WriteLine();
        WriteLine($"  Rounds Played: {_rounds}");
        WriteLine($"  Average Steps: {(float) _totalSteps / _rounds:F2}");
        WriteLine($"  Failures:      {_fails}");
        WriteLine();
        WriteLine("  Cheers!");
        WriteLine();
    }

    private void PlayGame(string expected)
    {
        var word = "AUDIO";

        var steps = 0;
        
        _solver.Reset();
        
        while (true)
        {
            steps++;
            
            var result = PlayStep(word);

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

    private StepResult PlayStep(string word)
    {
    }
}