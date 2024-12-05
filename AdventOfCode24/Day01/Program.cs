var pairs = File.ReadAllLines("input.txt")
                .Select(ParseLine);

var firstList = GetList(pairs, 0);
var secondList = GetList(pairs, 1);

var difference = firstList.Zip(secondList, (f, s) => Math.Abs(f - s)).Sum();

var similarityScore = firstList.Select(f => f * secondList.Count(s => s == f)).Sum();

Console.WriteLine(difference);
Console.WriteLine(similarityScore);

static int[] ParseLine(string line) =>
    line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

static int[] GetList(IEnumerable<int[]> pairs, int index) =>
    pairs.Select(p => p[index]).Order().ToArray();