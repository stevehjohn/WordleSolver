namespace WordleSolver.Infrastructure;

public class WordList
{
    private readonly List<string> _words = new();

    internal int WordCount => _words.Count;
    
    public WordList()
    {
        var words = File.ReadAllLines("Resources/words.txt");

        foreach (var word in words)
        {
            if (word.Length == 5)
            {
                var valid = true;
                
                foreach (var character in word)
                {
                    if (! char.IsLower(character))
                    {
                        valid = false;
                        
                        break;
                    }
                }

                if (valid)
                {
                    _words.Add(word);
                }
            }
        }
    }

    public HashSet<string> GetMatches(string correct, string available)
    {
        var matches = new HashSet<string>();

        foreach (var word in _words)
        {
            var valid = true;
            
            for (var i = 0; i < 5; i++)
            {
                if (correct[i] != ' ' && correct[i] != word[i])
                {
                    valid = false;
                }
            }

            if (valid)
            {
                matches.Add(word);
            }
        }
        
        return matches;
    }
}