namespace WordleSolver.Infrastructure;

public class WordListItem
{
    private readonly ulong _mask;

    public string Word { get; }

    public WordListItem(string word)
    {
        Word = word;

        for (var i = 0; i < word.Length; i++)
        {
            _mask |= 1ul << (word[i] - 'A');
        }
    }

    public bool Contains(char character)
    {
        return (_mask & 1ul << (character - 'A')) > 0;
    }
}