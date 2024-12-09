using System.Text.RegularExpressions;

static partial class Program
{
    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex DoMulRegex();

    public static void Main()
    {
        var input = File.ReadAllText("input.txt");

        var sum = MulRegex()
            .Matches(input)
            .Sum(Mul);

        Console.WriteLine(sum);

        var enable = true;
        var enabledSum = 0;
        foreach (Match match in DoMulRegex().Matches(input))
        {
            switch (match.Value)
            {
                case "do()":
                    enable = true;
                    break;

                case "don't()":
                    enable = false;
                    break;

                default:
                    if (enable)
                    {
                        enabledSum += Mul(match);
                    }
                    break;
            }
        }

        Console.WriteLine(enabledSum);
    }

    private static int Mul(Match match) =>
        int.Parse(match.Groups[1].Value) *
        int.Parse(match.Groups[2].Value);
}
