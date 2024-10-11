namespace WordleSolver.Infrastructure;

public class WordList
{
    private readonly List<WordListItem> _words = [];

    public IReadOnlyList<WordListItem> Words => _words;

    internal int WordCount => _words.Count;
    
    public WordList(WordSet wordSet, int length = 5)
    {
        var words = File.ReadAllLines($"Resources/{wordSet switch
        {
            WordSet.Comprehensive => "words",
            WordSet.OriginalAllowedAnswers => "original-allowed-answers",
            WordSet.OriginalAllowedGuesses => "original-allowed-guesses",
            WordSet.Scrabble => "scrabble",
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

        foreach (var word in list.Order())
        {
            _words.Add(new WordListItem(word));
        }
    }
}