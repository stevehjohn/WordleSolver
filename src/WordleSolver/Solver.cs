using WordleSolver.Infrastructure;

namespace WordleSolver;

public class Solver
{
    private readonly WordList _wordList;

    internal int WordCount => _wordList.WordCount;

    private readonly int _length;

    private char[] _correct;

    private List<Tile> _incorrect = [];

    private List<char> _excluded = [];
    
    public Solver(WordSet wordSet, int length = 5)
    {
        _wordList = new WordList(wordSet, length);

        _length = length;

        Reset();
    }

    public void SetCorrect(char letter, int position)
    {
        _correct[position] = letter;
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
        _correct = new char[_length];
        
        _incorrect.Clear();
        
        _excluded.Clear();
    }

    public HashSet<string> GetMatches()
    {
        var matches = new HashSet<string>();

        return matches;
    }

    public HashSet<string> GetMatches(string correct, string incorrect, string excluded)
    {
        var matches = new HashSet<string>();

        correct ??= string.Empty;

        incorrect ??= string.Empty;

        excluded ??= string.Empty;

        correct = correct.ToLower();

        incorrect = incorrect.ToLower();

        excluded = excluded.ToLower();

        foreach (var word in _wordList.Words)
        {
            var valid = true;
            
            for (var i = 0; i < _length; i++)
            {
                if (correct.Length > i && char.IsLetter(correct[i]) && correct[i] != word[i])
                {
                    valid = false;
                    
                    break;
                }

                if (excluded.Contains(word[i]))
                {
                    valid = false;
                    
                    break;
                }
            }

            if (valid)
            {
                matches.Add(word);
            }
        }

        var remove = new List<string>();
        
        foreach (var match in matches)
        {
            for (var i = 0; i < _length; i++)
            {
                if (incorrect.Length > i && char.IsLetter(incorrect[i]) && incorrect[i] == match[i])
                {
                    remove.Add(match);
                    
                    break;
                }

                if (incorrect.Length > i && char.IsLetter(incorrect[i]) && ! match.Contains(incorrect[i]))
                {
                    remove.Add(match);
                    
                    break;
                }
            }
        }

        foreach (var item in remove)
        {
            matches.Remove(item);
        }
        
        return matches.OrderByDescending(m => m.Distinct().Count()).ThenBy(m => m).ToHashSet();
    }
}