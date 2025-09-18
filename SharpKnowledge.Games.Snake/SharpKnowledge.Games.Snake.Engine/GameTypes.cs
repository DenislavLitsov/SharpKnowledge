namespace SharpKnowledge.Games.Snake.Engine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public enum GameState
{
    Playing,
    GameOver,
    Paused
}

public struct Position
{
    public int X { get; set; }
    public int Y { get; set; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Position position && X == position.X && Y == position.Y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(Position left, Position right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Position left, Position right)
    {
        return !(left == right);
    }
}