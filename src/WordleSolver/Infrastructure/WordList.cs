namespace WordleSolver.Infrastructure;

public class WordList
{
    private readonly List<string> _words;

    public IReadOnlyList<string> Words => _words;

    internal int WordCount => _words.Count;
    
    public WordList(WordSet wordSet, int length = 5)
    {
        var words = File.ReadAllLines($"Resources/{wordSet switch
        {
            WordSet.Comprehensive => "words",
            WordSet.OriginalAllowedAnswers => "original-allowed-answers",
            WordSet.OriginalAllowedGuesses => "original-allowed-guesses",
            _ => "english-words"
        }}.txt");
        
        var list = new List<string>();
        
        foreach (var word in words)
        {
            var lower = word.ToLower();
            
            if (lower.Length == length)
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
}