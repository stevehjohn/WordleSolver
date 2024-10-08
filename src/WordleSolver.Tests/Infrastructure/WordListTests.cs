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
}