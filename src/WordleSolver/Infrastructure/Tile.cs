namespace WordleSolver.Infrastructure;

public class Tile
{
    public char Character { get; }
    
    public int Position { get; }

    public Tile(char character, int position)
    {
        Character = character;
        
        Position = position;
    }
}