using System.Numerics;

var lines = File.ReadAllLines("input.txt");

var antennas = new List<Antenna>();

for (var y = 0; y < lines.Length; y++)
{
    for (var x = 0; x < lines[y].Length; x++)
    {
        if (lines[y][x] is char c && c != '.')
        {
            antennas.Add(new(new Position(x, y), c));
        }
    }
}

Console.WriteLine(
    CalculateAntinodePositions()
        .Where(IsInMap)
        .Distinct()
        .Count());

Console.WriteLine(
    CalculateAntinodePositionsWithResonanceHarmonics()
        .Where(IsInMap)
        .Distinct()
        .Count());

IEnumerable<Position> CalculateAntinodePositions()
{
    for (var a = 0; a < antennas.Count; a++)
    {
        for (var b = a + 1; b < antennas.Count; b++)
        {
            var antennaA = antennas[a];
            var antennaB = antennas[b];

            if (antennaA.Frequency == antennaB.Frequency)
            {
                var aToB = antennas[b].Position - antennas[a].Position;
                yield return antennas[a].Position - aToB;
                yield return antennas[b].Position + aToB;
            }
        }
    }
}

IEnumerable<Position> CalculateAntinodePositionsWithResonanceHarmonics()
{
    for (var a = 0; a < antennas.Count; a++)
    {
        for (var b = a + 1; b < antennas.Count; b++)
        {
            var antennaA = antennas[a];
            var antennaB = antennas[b];

            if (antennaA.Frequency == antennaB.Frequency)
            {
                var aToB = antennas[b].Position - antennas[a].Position;

                for (Position antinode = antennaA.Position; IsInMap(antinode); antinode -= aToB)
                {
                    yield return antinode;
                }

                for (Position antinode = antennaB.Position; IsInMap(antinode); antinode += aToB)
                {
                    yield return antinode;
                }
            }
        }
    }
}


bool IsInMap(Position position) =>
    position.X >= 0 && position.X < lines[0].Length && position.Y >= 0 && position.Y < lines.Length;

readonly record struct Position(int X, int Y) :
    IAdditionOperators<Position, Position, Position>,
    ISubtractionOperators<Position, Position, Position>
{
    public static Position operator +(Position left, Position right) =>
        new(left.X + right.X, left.Y + right.Y);

    public static Position operator -(Position left, Position right) =>
        new(left.X - right.X, left.Y - right.Y);
}

record Antenna(Position Position, char Frequency);