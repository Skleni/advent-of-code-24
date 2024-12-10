List<char?[,]> patterns1 = new()
{
    new char?[1,4]
    {
        { 'X', 'M', 'A', 'S' }
    },
    new char?[1,4]
    {
        { 'S', 'A', 'M', 'X' }
    },
    new char?[4,1]
    {
        { 'X' },
        { 'M' },
        { 'A' },
        { 'S' }
    },
    new char?[4,1]
    {
        { 'S' },
        { 'A' },
        { 'M' },
        { 'X' }
    },
    new char?[4,4]
    {
        { 'X', null, null, null },
        { null, 'M', null, null },
        { null, null, 'A', null },
        { null, null, null, 'S'}
    },
    new char?[4,4]
    {
        { 'S', null, null, null },
        { null, 'A', null, null },
        { null, null, 'M', null },
        { null, null, null, 'X'}
    },
    new char?[4,4]
    {
        { null, null, null, 'S' },
        { null, null, 'A', null },
        { null, 'M', null, null },
        { 'X', null, null, null}
    },
    new char?[4,4]
    {
        { null, null, null, 'X' },
        { null, null, 'M', null },
        { null, 'A', null, null },
        { 'S', null, null, null}
    }
};

List<char?[,]> patterns2 = new()
{
    new char?[3,3]
    {
        { 'M', null, 'S' },
        { null, 'A', null },
        { 'M', null, 'S' }
    },
    new char?[3,3]
    {
        { 'M', null, 'M' },
        { null, 'A', null },
        { 'S', null, 'S' }
    },
    new char?[3,3]
    {
        { 'S', null, 'M' },
        { null, 'A', null },
        { 'S', null, 'M' }
    },
    new char?[3,3]
    {
        { 'S', null, 'S' },
        { null, 'A', null },
        { 'M', null, 'M' }
    }
};

var rows = File.ReadAllLines("input.txt");

bool IsChar(int x, int y, char c) =>
    y >= 0 &&
    y < rows.Length &&
    x >= 0 &&
    x < rows[y].Length &&
    rows[y][x] == c;

bool HasPattern(int x, int y, char?[,] pattern)
{
    for (int i = 0; i < pattern.GetLength(0); i++)
    {
        for (int j = 0; j < pattern.GetLength(1); j++)
        {
            if (pattern[i, j].HasValue && !IsChar(x + j, y + i, pattern[i, j].Value))
            {
                return false;
            }
        }
    }

    return true;
}

int CountPatterns(IEnumerable<char?[,]> patterns)
{
    int count = 0;
    for (int y = 0; y < rows.Length; y++)
    {
        for (int x = 0; x < rows[y].Length; x++)
        {
            count += patterns.Count(p => HasPattern(x, y, p));
        }
    }

    return count;
}

Console.WriteLine(CountPatterns(patterns1));
Console.WriteLine(CountPatterns(patterns2));