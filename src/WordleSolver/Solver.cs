using System.Diagnostics.CodeAnalysis;
using WordleSolver.Extensions;
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

    public IEnumerable<string> GetMatches()
    {
        var matches = new List<WordListItem>();

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
        
        return matches.OrderByDescending(word => word.Word.CountDistinctCharacters()).ThenByDescending(word => word.Score).ThenBy(word => word.Word).Select(word => word.Word);
    }
    
    private bool CheckCorrect(WordListItem word)
    {
        foreach (var tile in _correct)
        {
            if (word.Word[tile.Position] != tile.Character)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckIncorrect(WordListItem word)
    {
        foreach (var tile in _incorrect)
        {
            if (word.Word[tile.Position] == tile.Character || ! word.Contains(tile.Character))
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckExcluded(WordListItem word)
    {
        for (var i = 0; i < word.Word.Length; i++)
        {
            if (_excluded.Contains(word.Word[i]))
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