using WordleSolver.Infrastructure;

namespace WordleSolver.Tests;

public class SolverTests
{
    private readonly Solver _solver = new(WordSet.Basic);
    
    [Theory]
    [InlineData("", "i,3|o,4", "aud", "bigot,biome,biros,bison,boils,coils,coins,corgi,envoi,foils,foist,gigot,going,hoist,icons,ingot,irons,irony,ivory," +
                                      "joins,joint,joist,kilos,kiosk,limos,lions,loins,minor,moist,noise,noisy,olive,omits,onion,opine,oriel,osier,ovine," +
                                      "owing,picot,pilot,piton,pitot,pivot,point,poise,prior,riots,roily,scion,silos,soils,toile,toils,torsi,trios,visor," +
                                      "voice,voile,winos")]
    [InlineData("t,4", "i,1|o,3", "bg", "foist,hoist,joint,joist,moist,point,posit,quoit,vomit")]
    [InlineData("o,1|i,2|t,4", "", "fs", "joint,point")]
    public void ReturnsExpectedWordsGivenConditions(string correct, string incorrect, string excluded, string expected)
    {
        ParseSetupParameter(correct).ForEach(p => _solver.SetCorrect(p.Character, p.Position));

        ParseSetupParameter(incorrect).ForEach(p => _solver.AddIncorrect(p.Character, p.Position));
        
        excluded.ToCharArray().ToList().ForEach(c => _solver.AddExcluded(c));

        var result = _solver.GetMatches();
        
        Assert.Equal(expected, string.Join(',', result));
    }

    private static List<Tile> ParseSetupParameter(string parameter)
    {
        var split = parameter.Split('|', StringSplitOptions.RemoveEmptyEntries);

        var result = new List<Tile>();
        
        foreach (var item in split)
        {
            var parts = item.Split(',');
            
            result.Add(new Tile(parts[0][0], int.Parse(parts[1])));
        }

        return result;
    }
}