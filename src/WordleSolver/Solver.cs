using System.Diagnostics.CodeAnalysis;
using WordleSolver.Infrastructure;

namespace WordleSolver;

public class Solver
{
    private readonly WordList _wordList;

    internal int WordCount => _wordList.WordCount;
    
    private readonly List<Tile> _correct = [];

    private readonly List<Tile> _incorrect = [];

    private readonly List<char> _excluded = [];

    private readonly int _length;
    
    public Solver(WordSet wordSet, int length = 5)
    {
        _wordList = new WordList(wordSet, length);

        _length = length;

        Reset();
    }

    public void SetCorrect(char letter, int position)
    {
        _correct.Add(new Tile(char.ToLower(letter), position));
    }

    public void AddIncorrect(char letter, int position)
    {
        _incorrect.Add(new Tile(char.ToLower(letter), position));
    }

    public void AddExcluded(char letter)
    {
        _excluded.Add(char.ToLower(letter));
    }

    public void Reset()
    {
        _correct.Clear();
        
        _incorrect.Clear();
        
        _excluded.Clear();
    }

    public string GetFirstMatch()
    {
        foreach (var word in _wordList.Words)
        {
            if (! CheckWord(word))
            {
                continue;
            }

            return word;
        }

        return null;
    }

    public IEnumerable<string> GetMatches()
    {
        var matches = new List<string>();

        foreach (var word in _wordList.Words)
        {
            if (! CheckWord(word))
            {
                continue;
            }

            matches.Add(word);
        }
        
        return matches.OrderByDescending(m => m.Distinct().Count()).ThenBy(m => m);
    }

    private bool CheckWord(string word)
    {
        if (! CheckCorrect(word))
        {
            return false;
        }
            
        if (! CheckIncorrect(word))
        {
            return false;
        }

        if (! CheckExcluded(word))
        {
            return false;
        }

        return true;
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
        foreach (var letter in word)
        {
            if (_excluded.Contains(letter))
            {
                return false;
            }
        }

        return true;
    }

    [ExcludeFromCodeCoverage]
    public void DumpState()
    {
        var colour = Console.ForegroundColor;
        
        Console.ForegroundColor = ConsoleColor.Green;
                
        for (var i = 0; i < _length; i++)
        {
            if (_correct.Any(t => t.Position == i))
            {
                Console.Write(_correct.Single(t => t.Position == i).Character);
                
                continue;
            }
            
            Console.Write(" ");
        }

        Console.Write("  ");

        Console.ForegroundColor = ConsoleColor.Yellow;
        
        foreach (var tile in _incorrect.DistinctBy(t => t.Character))
        {
            Console.Write($"{tile.Character} ");
        }

        Console.Write(" ");

        Console.ForegroundColor = ConsoleColor.Gray;
        
        foreach (var letter in _excluded.Distinct())
        {
            Console.Write($"{letter} ");
        }

        Console.ForegroundColor = colour;
    }
}