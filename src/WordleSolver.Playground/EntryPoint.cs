using WordleSolver.Playground.Toys;

namespace WordleSolver.Playground;

public static class EntryPoint
{
    public static void Main()
    {
        // var exerciser = new Excerciser();
        //
        // exerciser.RunAgainstAllWords();

        var finder = new StartWordFinder();
        
        finder.FindBestStartWord();
    }
}