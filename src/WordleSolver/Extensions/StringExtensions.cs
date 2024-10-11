using System.Numerics;

namespace WordleSolver.Extensions;

public static class StringExtensions
{
    public static int CountDistinctCharacters(this string word)
    {
        var bits = 0ul;
            
        for (var i = 0; i < word.Length; i++)
        {
            bits |= 1ul << (word[i] - 'A');
        }

        return BitOperations.PopCount(bits);
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