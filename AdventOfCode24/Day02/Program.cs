
var reports = File.ReadAllLines("input.txt")
                  .Select(ParseLine);

Console.WriteLine(reports.Count(r => IsSafe(r)));
Console.WriteLine(reports.Count(IsSafeWithDampener));

bool IsSafeWithDampener(int[] report)
{
    if (IsSafe(report))
    {
        return true;
    }

    var variations = Enumerable.Range(0, report.Length)
                               .Select(i => report.Take(i).Concat(report.Skip(i + 1)).ToArray());

    return variations.Any(v => IsSafe(v));
}

bool IsSafe(ReadOnlySpan<int> report)
{
    if (report.Length < 2)
    {
        return true;
    }

    int direction = Math.Sign(report[1] - report[0]);

    for (int i = 1; i < report.Length; i++)
    {
        var distance = report[i] - report[i - 1];

        if (Math.Sign(distance) != direction)
        {
            return false;
        }

        if (Math.Abs(distance) is < 1 or > 3)
        {
            return false;
        }
    }

    return true;
}

int[] ParseLine(string line) =>
    line.Split(' ').Select(int.Parse).ToArray();