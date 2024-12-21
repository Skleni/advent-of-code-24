using System.Collections.Immutable;

var lines = File.ReadAllLines("input.txt");
var equations = lines.Select(ParseEquation);

var allOperators = Enum.GetValues<Operator>();
var operatorsWithoutConcatenate = allOperators.Where(o => o != Operator.Concatenate);

Console.WriteLine(equations.Where(e => e.CouldBeTrue(operatorsWithoutConcatenate)).Sum(e => e.Result));
Console.WriteLine(equations.Where(e => e.CouldBeTrue(allOperators)).Sum(e => e.Result));

Equation ParseEquation(string line)
{
    var numbers = line
        .Split([':', ' '], StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToArray();

    return new Equation(
        numbers[0],
        numbers.Skip(1).ToArray());
}

record Equation(long Result, IReadOnlyList<long> Numbers)
{
    public bool CouldBeTrue(IEnumerable<Operator> availableOperators)
    {
        return CouldBeTrue([]);

        bool CouldBeTrue(ImmutableArray<Operator> operatorsSoFar)
        {
            if (operatorsSoFar.Length == this.Numbers.Count - 1)
            {
                return Evaluate(operatorsSoFar) == this.Result;
            }

            foreach (Operator nextOp in availableOperators)
            {
                if (CouldBeTrue(operatorsSoFar.Add(nextOp)))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public long? Evaluate(IReadOnlyList<Operator> operators)
    {
        if (operators.Count != this.Numbers.Count - 1)
        {
            return null;
        }

        var result = this.Numbers[0];
        for (int o = 0; o < operators.Count; o++)
        {
            result = operators[o] switch
            {
                Operator.Add => result + this.Numbers[o + 1],
                Operator.Multiply => result * this.Numbers[o + 1],
                Operator.Concatenate => long.Parse(result.ToString() + this.Numbers[o + 1].ToString()),
                _ => throw new InvalidOperationException()
            };
        }

        return result;
    }
}

enum Operator
{
    Add,
    Multiply,
    Concatenate
}