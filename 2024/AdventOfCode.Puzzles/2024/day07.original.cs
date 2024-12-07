using System.Data;
using System.Runtime.ExceptionServices;
using System.Text;

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
        input = """
        190: 10 19
        3267: 81 40 27
        83: 17 5
        156: 15 6
        7290: 6 8 6 15
        161011: 16 10 13
        192: 17 8 14
        21037: 9 7 18 13
        292: 11 6 16 20
        """.Split(Environment.NewLine);
        var overallResult = Solve(input, ['+', '*']);
        return overallResult.ToString();
    }

    public static string Part2(string[] input)
    {
        input = """
        190: 10 19
        3267: 81 40 27
        83: 17 5
        156: 15 6
        7290: 6 8 6 15
        161011: 16 10 13
        192: 17 8 14
        21037: 9 7 18 13
        292: 11 6 16 20
        """.Split(Environment.NewLine);
        var overallResult = Solve(input, ['+', '*', '|']);
        return overallResult.ToString();
    }

    private static ulong Solve(string[] input, char[] operators)
    {
        var (maxLen, equations) = Parse(input);
        var allCombos = GetOperatorCombinations(maxLen, operators);
        var overallResult = 0UL;
        foreach (var equation in equations)
        {
            var operatorCombos = allCombos[equation.Values.Length - 1];

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

    private static (int, List<Equation>) Parse(string[] input)
    {
        var equations = new List<Equation>(input.Length);
        var maxLen = 0;

        foreach (var line in input)
        {
            var idx = line.IndexOf(':');
            var answer = ulong.Parse(line[..idx]);
            var numList = line[(idx + 2)..];
            var numChars = numList.ToCharArray();
            var nums = numList.Split(' ').Select(int.Parse).ToArray();
            equations.Add(new Equation(answer, numChars, nums));

            if (numChars.Length > maxLen)
            {
                maxLen = numChars.Length;
            }
        }

        return (maxLen, equations);
    }

    internal record Equation(ulong Target, char[] ValueChars, int[] Values);

    private static Dictionary<int, List<string>> GetOperatorCombinations(int maxValue, char[] operators)
    {
        // Cache all possible permuations of the available operators
        var result = new Dictionary<int, List<string>>(maxValue);
        for (var i = 0; i < maxValue; i++)
        {
            var array = new char[i];
            result[i] = GetCombinations(array, operators);
        }

        return result;
    }

    private static List<string> GetCombinations(char[] array, char[] symbols)
    {
        var result = new List<string>();
        Combinations(array, symbols, 0, result);
        return result;
    }

    private static void Combinations(
        char[] array,
        char[] symbols,
        int index,
        List<string> result)
    {
        if (index == array.Length)
        {
            result.Add(new string(array));
            return;
        }

        foreach (char symbol in symbols)
        {
            array[index] = symbol;
            Combinations(array, symbols, index + 1, result);
        }
    }
}
