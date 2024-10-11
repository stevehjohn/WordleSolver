namespace WordleSolver.Infrastructure;

public class WordList
{
    private readonly List<WordListItem> _words = [];

    private readonly Dictionary<char, int> _frequencies = [];
    
    internal int WordCount => _words.Count;

    public IReadOnlyList<WordListItem> Words => _words;
    
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

        var frequencies = new Dictionary<char, int>();
        
        for (var c = 'a'; c <= 'z'; c++)
        {
            frequencies.Add(c, 0);
        }

        foreach (var word in list)
        {
            foreach (var character in word)
            {
                frequencies[character]++;
            }
        }

        var ordered = frequencies.OrderBy(f => f.Value);

        var score = 1;
        
        foreach (var item in ordered)
        {
            _frequencies.Add(item.Key, score);

            score++;
        }

        foreach (var word in list.Order())
        {
            score = 0;
            
            foreach (var character in word)
            {
                score *= _frequencies[character];
            }
            
            _words.Add(new WordListItem(word, score / 5));
        }
    }
}