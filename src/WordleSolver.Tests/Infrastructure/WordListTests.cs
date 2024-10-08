using WordleSolver.Infrastructure;

namespace WordleSolver.Tests.Infrastructure;

public class WordListTests
{
    private readonly WordList _wordList = new(WordSet.Basic);
    
    [Fact]
    public void LoadsExpectedWords()
    {
        Assert.Equal(4715, _wordList.WordCount);
    }

    [Theory]
    [InlineData("plan", "plane,plank,plano,plans,plant")]
    [InlineData(" lan", "blanc,bland,blank,clang,clank,clans,eland,flank,gland,llano,plane,plank,plano,plans,plant,slang,slant")]
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
    [InlineData("plan", "gt", "plane,plank,plano,plans")]
    [InlineData(" oin", "j", "coins,doing,going,loins,point")]
    public void ReturnsExpectedMatchesForCorrectLettersWithExclusions(string correct, string exclusions, string expected)
    {
        var result = _wordList.GetMatches(correct, null, exclusions);

        var list = expected.Split(',');
        
        Assert.Equal(list.Length, result.Count);

        foreach (var item in list)
        {
            Assert.Contains(item, result);
        }
    }

    [Theory]
    [InlineData(" lan", "----b", "blanc,bland,blank")]
    [InlineData(" oin", "t", "joint,point")]
    public void ReturnsExpectedMatchesForCorrectLettersWithAvailable(string correct, string incorrect, string expected)
    {
        var result = _wordList.GetMatches(correct, incorrect, null);

        var list = expected.Split(',');
        
        Assert.Equal(list.Length, result.Count);

        foreach (var item in list)
        {
            Assert.Contains(item, result);
        }
    }

    [Theory]
    [InlineData("", "---io", "aud", "bigot,biome,biros,bison,boils,coils,coins,corgi,envoi,foils,foist,hoist,icons,ingot,irons,irony," +
                                    "ivory,joins,joint,joist,kilos,limos,lions,loins,minor,moist,noise,noisy,olive,omits,opine,oriel," +
                                    "osier,ovine,owing,picot,pilot,piton,pivot,point,poise,riots,roily,scion,toile,toils,torsi,trios," +
                                    "visor,voice,voile,winos,gigot,going,kiosk,pitot,prior,silos,soils,onion")]
    [InlineData("----t", "-i-o", "audbg", "foist,hoist,joint,joist,moist,point,posit,vomit")]
    [InlineData("-oi-t", "", "audbgfs", "joint,point")]
    public void SimulateRealGameProgression(string correct, string incorrect, string excluded, string expected)
    {
        var result = _wordList.GetMatches(correct, incorrect, excluded);

        var list = expected.Split(',');
        
        Assert.Equal(list.Length, result.Count);

        foreach (var item in list)
        {
            Assert.Contains(item, result);
        }
    }
}