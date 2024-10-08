using WordleSolver.Infrastructure;

namespace WordleSolver.Tests.Infrastructure;

public class WordListTests
{
    private readonly WordList _wordList = new();
    
    [Fact]
    public void LoadsExpectedWords()
    {
        Assert.Equal(12927, _wordList.WordCount);
    }

    [Theory]
    [InlineData("plan ", "plane,plang,plans,plant")]
    public void ReturnsExpectedMatchesForCorrectLetters(string correct, string expected)
    {
        var result = _wordList.GetMatches(correct, null);

        var list = expected.Split(',');

        foreach (var item in list)
        {
            Assert.Contains(item, result);
        }
    }
}