using WordleSolver.Infrastructure;

namespace WordleSolver.Tests.Infrastructure;

public class WordListTests
{
    private readonly WordList _wordList = new(WordSet.Comprehensive);
    
    [Fact]
    public void LoadsExpectedWords()
    {
        Assert.Equal(21952, _wordList.WordCount);
    }

    [Theory]
    [InlineData("plan ", "plana,plane,plang,plank,plano,plans,plant")]
    [InlineData(" lan ", "alana,aland,alane,alang,alani,alano,alans,alant,blanc,bland,blane,blank,clang,clank,clans,elana,eland,elane,elans,flane,flang,flank,flann,flans,gland,glans,ilana,klans,llano,oland,plana,plane,plang,plank,plano,plans,plant,slane,slang,slank,slant,ulana,ulane,ulani,ulans")]
    public void ReturnsExpectedMatchesForCorrectLetters(string correct, string expected)
    {
        var result = _wordList.GetMatches(correct, null, null);

        var list = expected.Split(',');
        
        Assert.Equal(list.Length, result.Count);

        foreach (var item in list)
        {
            Assert.Contains(item, result);
        }
    }

    [Theory]
    [InlineData("plan ", "gt", "plane,plans")]
    public void ReturnsExpectedMatchesForCorrectLettersWithExclusions(string correct, string exclusions, string expected)
    {
        var result = _wordList.GetMatches(correct, null, exclusions);

        var list = expected.Split(',');

        foreach (var item in list)
        {
            Assert.Contains(item, result);
        }
    }
}