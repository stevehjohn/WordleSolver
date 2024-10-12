using WordleSolver.Infrastructure;

namespace WordleSolver.Tests;

public class SolverTests
{
    private readonly Solver _solver = new(WordSet.Basic);

    [Fact]
    public void FindsTheCorrectNumberOfWords()
    {
        Assert.Equal(4715, _solver.WordCount);
    }

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
        
        Assert.Equal("bigot,biome,biros,bison,boils,coils,coins,corgi,envoi,foils,foist,hoist,icons,ingot,irons,irony,ivory,joins,joint," +
                     "joist,kilos,limos,lions,loins,minor,moist,noise,noisy,olive,omits,opine,oriel,osier,ovine,owing,picot,pilot,piton," +
                     "pivot,point,poise,riots,roily,scion,toile,toils,torsi,trios,visor,voice,voile,winos,gigot,going,kiosk,pitot,prior," +
                     "silos,soils,onion", string.Join(',', result));
        
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
        
        Assert.Equal("bacon,baron,baton,bloat,boast,boats,bolas,borax,boyar,canoe,capon,carob,carol,carom,chaos,cloak,coals,coast,coats," +
                     "cobra,colza,comas,conga,copal,copra,coral,croak,float,flora,foals,foams,foamy,focal,foray,forma,fovea,gator,gloat," +
                     "goals,goats,goral,groan,groat,halos,havoc,hoary,horal,jabot,loach,loafs,loamy,loans,loath,lobar,loran,major,manor," +
                     "mason,mayor,moans,moats,mocha,molar,moral,moray,novae,novas,oaken,oaten,oaths,ocean,octal,okays,omega,opals,opera," +
                     "orals,orate,organ,ovals,ovary,ovate,poach,polar,polka,rayon,roach,roams,roast,royal,sabot,salon,shoal,shoat,soapy," +
                     "solar,sonar,stoma,tabor,tacos,talon,taros,tonal,topaz,vocal,wagon,woman,yapok,zonal,cabob,canon,coach,comma,cotta," +
                     "fanon,kabob,kapok,koala,local,loyal,nabob,oases,offal,razor,roars,soaks,soaps,soars,sofas,stoat,tarot,toast,total," +
                     "cocoa,momma,poppa", string.Join(',', result));
        
        _solver.AddIncorrect('a', 1);
        _solver.AddIncorrect('o', 3);
        _solver.AddExcluded('b');
        _solver.AddExcluded('c');
        _solver.AddExcluded('n');

        result = _solver.GetMatches();
        
        Assert.Equal("float,flora,foals,foams,foamy,foray,forma,fovea,gloat,goals,goats,goral,groat,hoary,horal,loafs,loamy,loath,moats," +
                     "molar,moral,moray,okays,omega,opals,opera,orals,orate,ovals,ovary,ovate,polar,polka,roams,roast,royal,shoal,shoat," +
                     "soapy,solar,stoma,topaz,koala,loyal,offal,roars,soaks,soaps,soars,sofas,stoat,toast,total,momma,poppa", string.Join(',', result));
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
        
        Assert.Equal("baits,baize,cairn,china,cigar,fails,faint,fairs,fairy,faith,fiats,final,firma,gaily,gains,gaits,giant,hails,hairs," +
                     "hairy,ihram,ileac,image,inapt,infra,inlay,intra,irate,jails,lairs,laity,liars,liras,mails,mains,maize,minas,nails," +
                     "naive,pails,pains,paint,pairs,phial,pieta,pilaf,prima,rails,rains,rainy,raise,rival,riyal,saint,sigma,sitar,swami," +
                     "tails,tarsi,triac,trial,vials,vicar,viral,vista,vitae,vital,wails,waist,waits,waive,witan,briar,cacti,circa,friar," +
                     "iliac,inane,khaki,lanai,liana,lilac,maims,pizza,rabbi,sails,sisal,taiga,taint,tiara,titan,villa,visas,wirra", string.Join(',', result));
        
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
        
        Assert.Equal("biota,china,infra,intra,viola,vista,voila,circa,liana,pizza,taiga,tiara,villa,wirra", string.Join(',', result));
        
        _solver.SetCorrect('i', 1);
        _solver.SetCorrect('o', 2);
        _solver.AddExcluded('b');
        _solver.AddExcluded('t');

        result = _solver.GetMatches();
        
        Assert.Equal("viola", string.Join(',', result));
    }
    
    [Fact]
    public void ReturnsExpectedWordsGivenConditions05()
    {
        _solver.Reset();
        
        _solver.SetCorrect('i', 3);
        _solver.AddExcluded('a');
        _solver.AddExcluded('u');
        _solver.AddExcluded('d');
        _solver.AddExcluded('o');

        var result = _solver.GetMatches();
        
        Assert.Equal("befit,begin,cylix,elfin,ethic,helix,legit,lenis,lexis,lyric,merit,mycin,penis,peril,pewit,refit,rejig,relic,remit," +
                     "remix,resin,scrim,scrip,serif,sheik,skein,split,sprig,sprit,stein,strip,their,xeric,belie,civil,cynic,eyrie,eyrir," +
                     "finis,genie,genii,infix,kiwis,licit,limit,lxvii,minis,petit,pixie,ricin,vigil,visit,xcvii,civic,eerie,immix,mimic," +
                     "minim,pipit,xviii,xxvii,xxxiv,xxiii,xxxii,xxxix", string.Join(',', result));
        
        _solver.AddIncorrect('e', 1);
        _solver.AddIncorrect('t', 4);
        _solver.AddExcluded('b');
        _solver.AddExcluded('f');

        result = _solver.GetMatches();
        
        Assert.Equal("ethic,stein,their", string.Join(',', result));
        
        _solver.AddIncorrect('e', 0);
        _solver.AddIncorrect('t', 1);
        _solver.AddIncorrect('h', 2);
        _solver.AddExcluded('c');

        result = _solver.GetMatches();
        
        Assert.Equal("their", string.Join(',', result));
    }

    [Fact]
    public void SolverReturnsCorrectState()
    {
        _solver.Reset();
        
        _solver.SetCorrect('a', 0);
        _solver.SetCorrect('b', 1);
        
        _solver.AddIncorrect('r', 2);
        
        _solver.AddExcluded('x');
        _solver.AddExcluded('y');
        _solver.AddExcluded('z');

        Assert.Contains('x', _solver.Excluded);
        Assert.Contains('y', _solver.Excluded);
        Assert.Contains('z', _solver.Excluded);
    }
}