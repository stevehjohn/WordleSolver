namespace WordleSolver.Infrastructure;

public class WordList
{
    private readonly List<string> _words;

    internal int WordCount => _words.Count;
    
    public WordList(WordSet wordSet)
    {
        var words = File.ReadAllLines(wordSet == WordSet.Comprehensive ? "Resources/words.txt" :  "Resources/english-words.txt");

        var list = new List<string>();
        
        foreach (var word in words)
        {
            var lower = word.ToLower();
            
            if (lower.Length == 5)
            {
                var valid = true;
                
                foreach (var character in lower)
                {
                    if (! char.IsLetter(character))
                    {
                        valid = false;
                        
                        break;
                    }
                }

                if (valid)
                {
                    list.Add(lower);
                }
            }
        }

        _words = list.Order().ToList();
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

        foreach (var word in _words)
        {
            var valid = true;
            
            for (var i = 0; i < 5; i++)
            {
                if (correct.Length > i && correct[i] != ' ' && correct[i] != word[i])
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
            for (var i = 0; i < 5; i++)
            {
                if (incorrect.Length > i && ! match.Contains(incorrect[i]))
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
        
        return matches;
    }
}