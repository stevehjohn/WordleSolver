using WordleSolver.Infrastructure;

namespace WordleSolver.Playground.Toys;

public class Excerciser
{
    private readonly WordList _wordList = new(WordSet.Basic);

    private readonly Solver _solver = new Solver(WordSet.Basic);
    
    public void RunAgainstAllWords()
    {
        foreach (var word in _wordList.Words)
        {
            
        }
    }
}