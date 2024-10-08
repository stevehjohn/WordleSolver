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
    
    [Fact]
    public void ReturnsExpectedWordsGivenConditions02()
    {
        _solver.Reset();
        
        _solver.AddIncorrect('a', 0);
        _solver.AddIncorrect('o', 4);
        _solver.AddExcluded('u');
        _solver.AddExcluded('d');
        _solver.AddExcluded('i');

        var result = _solver.GetMatches();
        
        Assert.Equal("bacon,baron,baton,bloat,boast,boats,bolas,borax,boyar,cabob,canoe,canon,capon,carob,carol,carom,chaos,cloak,coach," +
                     "coals,coast,coats,cobra,cocoa,colza,comas,comma,conga,copal,copra,coral,cotta,croak,fanon,float,flora,foals,foams," +
                     "foamy,focal,foray,forma,fovea,gator,gloat,goals,goats,goral,groan,groat,halos,havoc,hoary,horal,jabot,kabob,kapok," +
                     "koala,loach,loafs,loamy,loans,loath,lobar,local,loran,loyal,major,manor,mason,mayor,moans,moats,mocha,molar,momma," +
                     "moral,moray,nabob,novae,novas,oaken,oases,oaten,oaths,ocean,octal,offal,okays,omega,opals,opera,orals,orate,organ," +
                     "ovals,ovary,ovate,poach,polar,polka,poppa,rayon,razor,roach,roams,roars,roast,royal,sabot,salon,shoal,shoat,soaks," +
                     "soaps,soapy,soars,sofas,solar,sonar,stoat,stoma,tabor,tacos,talon,taros,tarot,toast,tonal,topaz,total,vocal,wagon," +
                     "woman,yapok,zonal", string.Join(',', result));
        
        _solver.AddIncorrect('a', 1);
        _solver.AddIncorrect('o', 3);
        _solver.AddExcluded('b');
        _solver.AddExcluded('c');
        _solver.AddExcluded('n');

        result = _solver.GetMatches();
        
        Assert.Equal("float,flora,foals,foams,foamy,foray,forma,fovea,gloat,goals,goats,goral,groat,hoary,horal,koala,loafs,loamy,loath," +
                     "loyal,moats,molar,momma,moral,moray,offal,okays,omega,opals,opera,orals,orate,ovals,ovary,ovate,polar,polka,poppa," +
                     "roams,roars,roast,royal,shoal,shoat,soaks,soaps,soapy,soars,sofas,solar,stoat,stoma,toast,topaz,total", string.Join(',', result));
    }
    
    [Fact]
    public void ReturnsExpectedWordsGivenConditions03()
    {
        _solver.Reset();
        
        _solver.AddIncorrect('a', 0);
        _solver.AddIncorrect('i', 3);
        _solver.AddExcluded('u');
        _solver.AddExcluded('d');
        _solver.AddExcluded('o');

        var result = _solver.GetMatches();
        
        Assert.Equal("baits,baize,briar,cacti,cairn,china,cigar,circa,fails,faint,fairs,fairy,faith,fiats,final,firma,friar,gaily,gains," +
                     "gaits,giant,hails,hairs,hairy,ihram,ileac,iliac,image,inane,inapt,infra,inlay,intra,irate,jails,khaki,lairs,laity," +
                     "lanai,liana,liars,lilac,liras,mails,maims,mains,maize,minas,nails,naive,pails,pains,paint,pairs,phial,pieta,pilaf," +
                     "pizza,prima,rabbi,rails,rains,rainy,raise,rival,riyal,sails,saint,sigma,sisal,sitar,swami,taiga,tails,taint,tarsi," +
                     "tiara,titan,triac,trial,vials,vicar,villa,viral,visas,vista,vitae,vital,wails,waist,waits,waive,wirra,witan", string.Join(',', result));
        
        _solver.SetCorrect('a', 1);
        _solver.SetCorrect('i', 2);
        _solver.AddIncorrect('t', 3);
        _solver.AddIncorrect('s', 4);
        _solver.AddExcluded('b');

        result = _solver.GetMatches();
        
        Assert.Equal("saint,waist", string.Join(',', result));
    }
    
    [Fact]
    public void ReturnsExpectedWordsGivenConditions04()
    {
        _solver.Reset();
        
        _solver.SetCorrect('a', 4);
        _solver.AddIncorrect('i', 3);
        _solver.AddExcluded('m');
        _solver.AddExcluded('e');
        _solver.AddExcluded('d');

        var result = _solver.GetMatches();
        
        Assert.Equal("biota,china,circa,infra,intra,liana,pizza,taiga,tiara,villa,viola,vista,voila,wirra", string.Join(',', result));
        
        _solver.SetCorrect('i', 1);
        _solver.SetCorrect('o', 2);
        _solver.AddExcluded('b');
        _solver.AddExcluded('t');

        result = _solver.GetMatches();
        
        Assert.Equal("viola", string.Join(',', result));
    }
}