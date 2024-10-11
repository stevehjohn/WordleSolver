namespace WordleSolver.Infrastructure;

public class Tile
{
    public override int GetHashCode()
    {
        return HashCode.Combine(Character, Position);
    }

    public char Character { get; }
    
    public int Position { get; }

    public Tile(char character, int position)
    {
        Character = character;
        
        Position = position;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }
        
        return Equals((Tile) obj);
    }
    
    private bool Equals(Tile other)
    {
        return Character == other.Character && Position == other.Position;
    }
}