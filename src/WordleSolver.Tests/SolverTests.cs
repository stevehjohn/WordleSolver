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