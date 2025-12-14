namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 07, CodeType.Original)]
public class Day_07_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var overallResult = Solve(input, ['+', '*']);
        return overallResult.ToString();
    }

    public static string Part2(string[] input)
    {
        var overallResult = Solve(input, ['+', '*', '|']);
        return overallResult.ToString();
    }

    private static ulong Solve(string[] input, char[] operators)
    {
        var equations = Parse(input);
        var overallResult = 0UL;
        foreach (var equation in equations)
        {
            var operatorCombos = GetCombinations(new char[equation.Values.Length - 1], operators);

            foreach (var combo in operatorCombos)
            {
                var currOp = 0;

                var stack = new Stack<ulong>();
                for (var i = equation.Values.Length - 1; i >= 0; i--)
                {
                    stack.Push((ulong)equation.Values[i]);
                }

                while (stack.Count > 1)
                {
                    var first = stack.Pop();
                    var second = stack.Pop();
                    var op = combo[currOp++];

                    var temp = op switch
                    {
                        '+' => first + second,
                        '*' => first * second,
                        '|' => ulong.Parse($"{first}{second}"),
                        _ => throw new InvalidOperationException("Unexpected input"),
                    };

                    stack.Push(temp);
                }

                var result = stack.Pop();
                if (result == equation.Target)
                {
                    overallResult += equation.Target;
                    break;
                }
            }
        }

        return overallResult;
    }

    private static List<Equation> Parse(string[] input)
    {
        var equations = new List<Equation>(input.Length);

        foreach (var line in input)
        {
            var idx = line.IndexOf(':');
            var answer = ulong.Parse(line[..idx]);
            var numList = line[(idx + 2)..];
            var numChars = numList.ToCharArray();
            var nums = numList.Split(' ').Select(int.Parse).ToArray();
            equations.Add(new Equation(answer, numChars, nums));
        }

        return equations;
    }

    internal sealed record Equation(ulong Target, char[] ValueChars, int[] Values);

    private static IEnumerable<string> GetCombinations(char[] array, char[] symbols)
    {
        foreach (var combo in Combinations(array, symbols, 0))
        {
            yield return combo;
        }
    }

    private static IEnumerable<string> Combinations(
        char[] array,
        char[] symbols,
        int index)
    {
        if (index == array.Length)
        {
            yield return new string(array);
            yield break;
        }

        foreach (var symbol in symbols)
        {
            array[index] = symbol;
            foreach (var combination in Combinations(array, symbols, index + 1))
            {
                yield return combination;
            }
        }
    }
}
