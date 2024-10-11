namespace WordleSolver.Extensions;

public static class StringExtensions
{
    public static int CountDistinctCharacters(this string word)
    {
        var bits = 0L;
            
        var count = 0;
            
        for (var i = 0; i < word.Length; i++)
        {
            var mask = 1L << (word[i] & 63);
                
            if ((bits & mask) == 0)
            {
                bits |= mask;
                    
                count++;
            }
        }

        return count;
    }

    public static bool ContainsCharacter(this string word, char character)
    {
        for (var i = 0; i < word.Length; i++)
        {
            if (word[i] == character)
            {
                return true;
            }
        }

        return false;
    }
}