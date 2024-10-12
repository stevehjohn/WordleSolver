using WordleSolver.Extensions;

namespace WordleSolver.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("steve", 's', true)]
    [InlineData("steve", 'z', false)]
    [InlineData("steve", 'e', true)]
    [InlineData("steve", 't', true)]
    [InlineData("steve", 'v', true)]
    public void ContainsCharacterReturnsCorrectValue(string word, char character, bool expected)
    {
        Assert.Equal(expected, word.ContainsCharacter(character));
    }
}