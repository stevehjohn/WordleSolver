namespace WordleSolver.Infrastructure;

public class WordListItem
{
    private readonly ulong _mask;

    public string Word { get; }
    
    public int Score { get; }

    public WordListItem(string word, int score)
    {
        Word = word;

        for (var i = 0; i < word.Length; i++)
        {
            _mask |= 1ul << (word[i] - 'A');
        }

        Score = score;
    }

    public bool Contains(char character)
    {
        return (_mask & 1ul << (character - 'A')) > 0;
    }

    public override string ToString()
    {
        return $"{Word}:{Score}";
    }
}