var lines = File.ReadAllLines("input.txt");

Map map = new(new Tile[lines.Length, lines[0].Length]);
Position startingPosition = default;

for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < lines[y].Length; x++)
    {
        switch (lines[y][x])
        {
            case '^':
                startingPosition = new Position(x, y);
                break;
            case '#':
                map.AddObstacle(new Position(x, y));
                break;
        }
    }
}

var guard = new Guard(map, startingPosition, Direction.Up);
guard.MoveUntilOutOfMapOrInLoop();

Console.WriteLine(guard
    .VisitedPositions
    .Select(p => p.Position)
    .Distinct()
    .Count());

Console.WriteLine(FindObstaclePositions(guard).Distinct().Count());

IEnumerable<Position> FindObstaclePositions(Guard guard)
{
    foreach (var position in guard.VisitedPositions.Skip(1).SkipLast(1))
    {
        var possibleObstaclePosition = position.Position + position.LeftTowards;
        map.AddObstacle(possibleObstaclePosition);

        var alternateRealityGuard = new Guard(map, startingPosition, Direction.Up);
        alternateRealityGuard.MoveUntilOutOfMapOrInLoop();

        map.RemoveObstacle(possibleObstaclePosition);

        if (map.Contains(alternateRealityGuard.Position))
        {
            yield return possibleObstaclePosition;
        }
    }
}

class Map(Tile[,] tiles)
{
    private Tile[,] tiles = tiles;

    public Tile this[Position position] =>
        tiles[position.Y, position.X];

    public void AddObstacle(Position position) =>
        tiles[position.Y, position.X] = Tile.Obstacle;

    public void RemoveObstacle(Position position) =>
        tiles[position.Y, position.X] = Tile.Empty;

    public bool Contains(Position position) =>
        position.X >= 0 && position.X < tiles.GetLength(1) && position.Y >= 0 && position.Y < tiles.GetLength(0);
}

readonly record struct Position(int X, int Y)
{
    public static Position operator +(Position p, Direction d) => d switch
    {
        Direction.Up => new Position(p.X, p.Y - 1),
        Direction.Right => new Position(p.X + 1, p.Y),
        Direction.Down => new Position(p.X, p.Y + 1),
        _ => new Position(p.X - 1, p.Y),
    };
}

enum Tile
{
    Empty,
    Obstacle
}

enum Direction
{
    Up, Right, Down, Left
}

static class DirectionExtensions
{
    public static Direction Next(this Direction direction) =>
        (Direction)(((int)direction + 1) % Enum.GetValues<Direction>().Length);
}

class Guard(Map map, Position position, Direction direction)
{
    private List<(Position Position, Direction LeftTowards)> visitedPositions = new();

    public Position Position
    {
        get;
        private set
        {
            visitedPositions.Add((field, Direction));
            field = value;
        }
    } = position;

    public Direction Direction { get; private set; } = direction;

    public IReadOnlyList<(Position Position, Direction LeftTowards)> VisitedPositions => visitedPositions;

    public bool HasLeft =>
        !map.Contains(Position);

    public void TurnRight() =>
        Direction = Direction.Next();

    private void EnsureCanMove()
    {
        var nextPosition = Position + Direction;
        while (map.Contains(nextPosition) && map[nextPosition] != Tile.Empty)
        {
            TurnRight();
            nextPosition = Position + Direction;
        }
    }

    private void Move()
    {
        EnsureCanMove();
        Position += Direction;
    }

    public void MoveUntilOutOfMapOrInLoop()
    {
        while (map.Contains(Position))
        {
            Move();
            EnsureCanMove();

            if (VisitedPositions.Contains((Position, Direction)))
            {
                return;
            }
        }
    }
}