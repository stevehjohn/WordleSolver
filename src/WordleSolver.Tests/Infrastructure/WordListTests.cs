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
    [InlineData(" lan", "b", "blanc,bland,blank")]
    [InlineData(" oin", "t", "joint,point")]
    public void ReturnsExpectedMatchesForCorrectLettersWithAvailable(string correct, string available, string expected)
    {
        var result = _wordList.GetMatches(correct, available, null);

        var list = expected.Split(',');
        
        Assert.Equal(list.Length, result.Count);

        foreach (var item in list)
        {
            Assert.Contains(item, result);
        }
    }

    [Theory]
    [InlineData("", "io", "aud", "bigot,bimbo,bingo,biome,biros,bison,bogie,boils,boric,bowie,broil,chino,choir,coils,coins,colic,comic,conic,corgi,envoi,eosin,fibro,foils,foist,folic,folio,gigot,gismo,gizmo,going,groin,hippo,hoist,icons,igloo,ingot,intro,ionic,irons,irony,ivory,jingo,joins,joint,joist,kilos,kiosk,limbo,limos,lingo,lions,litho,logic,login,loins,micro,minor,moist,motif,movie,moxie,nitro,noise,noisy,oleic,olive,omits,onion,opine,optic,orbit,oriel,osier,ovine,owing,picot,piezo,pilot,pinto,piton,pitot,pivot,point,poise,polio,polis,posit,primo,prior,rhino,riots,robin,roily,rosin,scion,silos,socio,soils,sonic,spoil,stoic,toile,toils,tonic,topic,toric,torsi,toxic,toxin,trios,vireo,visor,vitro,voice,voile,vomit,winos,yogic")]
    [InlineData("    t", "io", "audbg", "foist,hoist,joint,joist,moist,picot,pilot,pitot,pivot,point,posit,vomit")]
    [InlineData(" oi t", "", "audbgfs", "joint,point")]
    [InlineData(" oint", "", "audbgfsp", "joint")]
    public void SimulateRealGameProgression(string correct, string available, string excluded, string expected)
    {
        var result = _wordList.GetMatches(correct, available, excluded);

        var list = expected.Split(',');
        
        Assert.Equal(list.Length, result.Count);

        foreach (var item in list)
        {
            Assert.Contains(item, result);
        }
    }
}