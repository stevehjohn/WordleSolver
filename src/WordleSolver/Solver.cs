using WordleSolver.Infrastructure;

namespace WordleSolver;

public class Solver
{
    private readonly WordList _wordList;

    internal int WordCount => _wordList.WordCount;
    
    private readonly List<Tile> _correct = [];

    private readonly List<Tile> _incorrect = [];

    private readonly List<char> _excluded = [];
    
    public Solver(WordSet wordSet, int length = 5)
    {
        _wordList = new WordList(wordSet, length);

        Reset();
    }

    public void SetCorrect(char letter, int position)
    {
        // TODO: Should this throw and insist on a reset?
        _correct.RemoveAll(t => t.Position == position);
        
        _correct.Add(new Tile(letter, position));
    }

    public void AddIncorrect(char letter, int position)
    {
        _incorrect.Add(new Tile(letter, position));
    }

    public void AddExcluded(char letter)
    {
        _excluded.Add(letter);
    }

    public void Reset()
    {
        _correct.Clear();
        
        _incorrect.Clear();
        
        _excluded.Clear();
    }

    public HashSet<string> GetMatches()
    {
        var matches = new HashSet<string>();

        foreach (var word in _wordList.Words)
        {
            if (! CheckCorrect(word))
            {
                continue;
            }
            
            if (! CheckIncorrect(word))
            {
                continue;
            }

            if (! CheckExcluded(word))
            {
                continue;
            }

            matches.Add(word);
        }
        
        return matches.OrderByDescending(m => m.Distinct().Count()).ThenBy(m => m).ToHashSet();
    }

    private bool CheckCorrect(string word)
    {
        foreach (var tile in _correct)
        {
            if (word[tile.Position] != tile.Character)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckIncorrect(string word)
    {
        foreach (var tile in _incorrect)
        {
            if (word[tile.Position] == tile.Character || ! word.Contains(tile.Character))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckExcluded(string word)
    {
        foreach (var letter in _excluded)
        {
            if (word.Contains(letter))
            {
                return false;
            }
        }

        return true;
    }
}