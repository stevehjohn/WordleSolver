using WordleSolver.Infrastructure;

namespace WordleSolver.Tests.Infrastructure;

public class WordListTests
{
    [Theory]
    [InlineData(WordSet.Basic, 4715)]
    [InlineData(WordSet.Comprehensive, 21952)]
    [InlineData(WordSet.OriginalAllowedAnswers, 2315)]
    [InlineData(WordSet.OriginalAllowedGuesses, 10657)]
    [InlineData(WordSet.Scrabble, 12972)]
    public void CanLoadAllWordLists(WordSet wordSet, int expectedCount)
    {
        var wordList = new WordList(wordSet);
        
        Assert.Equal(expectedCount, wordList.WordCount);
    }
}