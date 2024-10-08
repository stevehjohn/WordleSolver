using WordleSolver.Infrastructure;

namespace WordleSolver.Tests;

public class SolverTests
{
    private readonly Solver _solver = new(WordSet.Basic);
    
    [Fact]
    public void ReturnsExpectedWordsGivenConditions01()
    {
        _solver.Reset();
        
        _solver.AddIncorrect('i', 3);
        _solver.AddIncorrect('o', 4);
        _solver.AddExcluded('a');
        _solver.AddExcluded('u');
        _solver.AddExcluded('d');

        var result = _solver.GetMatches();
        
        Assert.Equal("bigot,biome,biros,bison,boils,coils,coins,corgi,envoi,foils,foist,gigot,going,hoist,icons,ingot,irons,irony,ivory," +
                     "joins,joint,joist,kilos,kiosk,limos,lions,loins,minor,moist,noise,noisy,olive,omits,onion,opine,oriel,osier,ovine," +
                     "owing,picot,pilot,piton,pitot,pivot,point,poise,prior,riots,roily,scion,silos,soils,toile,toils,torsi,trios,visor," +
                     "voice,voile,winos", string.Join(',', result));
        
        _solver.SetCorrect('t', 4);
        _solver.AddIncorrect('i', 1);
        _solver.AddIncorrect('o', 3);
        _solver.AddExcluded('b');
        _solver.AddExcluded('g');

        result = _solver.GetMatches();
        
        Assert.Equal("foist,hoist,joint,joist,moist,point", string.Join(',', result));
        
        _solver.SetCorrect('o', 1);
        _solver.SetCorrect('i', 2);
        _solver.AddExcluded('f');
        _solver.AddExcluded('s');

        result = _solver.GetMatches();
         
        Assert.Equal("joint,point", string.Join(',', result));
     }
}