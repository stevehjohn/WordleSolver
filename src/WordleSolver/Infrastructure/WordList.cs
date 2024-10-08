namespace WordleSolver.Infrastructure;

public class WordList
{
    private readonly List<string> _words = new();

    public WordList()
    {
        var words = File.ReadAllLines("Resources/english-words.txt");

        foreach (var word in words)
        {
            if (word.Length == 5)
            {
                _words.Add(word);
            }
        }
    }

    public List<string> GetMatches(string correct, string available)
    {
        var matches = new List<string>();

        foreach (var word in _words)
        {
            
        }
        
        return matches;
    }
}