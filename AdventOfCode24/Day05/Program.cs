var lines = File.ReadAllLines("input.txt");

var rulesStrings = lines
    .TakeWhile(l => l != string.Empty);

var rules = rulesStrings
    .Select(ParseRule)
    .ToArray();

var rulesByPage = rules
    .GroupBy(r => r.Before)
    .ToDictionary(
        g => g.Key,
        g => g.Order().ToArray());

var updates = lines
    .Skip(rules.Length + 1)
    .Select(ParseUpdate)
    .GroupBy(IsCorrect)
    .ToArray();

var correctUpdates = updates
    .Single(g => g.Key == true);

var incorrectUpdates = updates
    .Single(g => g.Key == false);

Console.WriteLine(correctUpdates.Sum(MiddlePage));

Console.WriteLine(incorrectUpdates.Select(FixIncorrectUpdate).Sum(MiddlePage));

(int Before, int After) ParseRule(string rule) =>
    rule.Split("|") switch
    {
    [string before, string after] => (int.Parse(before), int.Parse(after)),
        _ => throw new ArgumentOutOfRangeException(nameof(rule))
    };

List<int> ParseUpdate(string update) =>
    update.Split(",").Select(int.Parse).ToList();

bool IsCorrect(List<int> update) =>
    rules.All(r => (update.IndexOf(r.Before), update.IndexOf(r.After)) switch
    {
        (-1, _) => true,
        (_, -1) => true,
        (int before, int after) => before < after
    });

List<int> FixIncorrectUpdate(List<int> update) =>
    update
        .Order(new RuleComparer(rulesByPage, update))
        .ToList();


int MiddlePage(List<int> update) =>
    update[update.Count / 2];

class RuleComparer(IReadOnlyDictionary<int, (int Before, int After)[]> rulesByPage, List<int> update) : IComparer<int>
{
    public int Compare(int x, int y)
    {
        if (rulesByPage.TryGetValue(x, out var xRules) && xRules.Any(r => r.After == y))
        {
            return -1;
        }

        if (rulesByPage.TryGetValue(y, out var yRules) && yRules.Any(r => r.After == x))
        {
            return 1;
        }

        return 0;
    }
}