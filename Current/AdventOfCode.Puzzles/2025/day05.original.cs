using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 05, CodeType.Original)]
public class Day_05_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var answer = 0;

        /*
        input = """
        3-5
        10-14
        16-20
        12-18

        1
        5
        8
        11
        17
        32
        """.Split(Environment.NewLine);
        */

        var freshRanges = GetRanges(input);
        var ingredientIds = GetIngredientIDs(input);
        var freshIngredients = new HashSet<ulong>();

        foreach (var range in freshRanges)
        {
            foreach (var id in ingredientIds)
            {

                if (IsInRange(range, id))
                {
                    freshIngredients.Add(id);
                }
            }
        }

        answer = freshIngredients.Count;

        return answer.ToString();
    }

    public static string Part2(string[] input)
    {
        var answer = 0UL;

        /*
        input = """
        3-5
        10-14
        16-20
        12-18

        1
        5
        8
        11
        17
        32
        """.Split(Environment.NewLine);
        */

        var freshRanges = GetRanges(input);

        while (true)
        {
            var totalMerged = 0;

            freshRanges.Sort();

            for (var i = 0; i < freshRanges.Count - 1; i++)
            {
                var a = freshRanges[i];
                var b = freshRanges[i + 1];

                if (TryMerge(a, b, out var merged))
                {
                    totalMerged++;
                    freshRanges.RemoveAt(i);
                    freshRanges.RemoveAt(i);
                    freshRanges.Add(merged.Value);
                    Console.WriteLine($"Merged {a}, {b} -> {merged}");
                }
                else
                {
                    Console.WriteLine($"Could not merge {a} and {b}");
                }
            }

            if (totalMerged == 0)
            {
                break;
            }
        }

        Console.WriteLine($"-----");
        foreach (var range in freshRanges)
        {
            var total = 1 + range.Item2 - range.Item1;
            Console.WriteLine($"{range} has {total} items");
            answer += total;
        }

        return answer.ToString();
    }


    private static bool TryMerge(
        (ulong, ulong) a,
        (ulong, ulong) b,
        [NotNullWhen(true)] out (ulong, ulong)? merged)
    {
        merged = null;

        var aLower = a.Item1;
        var aUpper = a.Item2;
        var bLower = b.Item1;
        var bUpper = b.Item2;
        ulong? newLower = null;
        ulong? newUpper = null;

        if (aLower < bLower && aUpper < bLower)
        {
            return false;
        }
        else if (bLower < aLower && bUpper < aLower)
        {
            return false;
        }
        else if (bLower >= aLower && bUpper <= aUpper)
        {
            newLower = aLower;
            newUpper = aUpper;
        }
        else if (bLower >= aLower && aUpper <= bUpper)
        {
            newLower = aLower;
            newUpper = bUpper;
        }

        if (newLower is null && newUpper is null)
        {
            return false;
        }

        merged = (newLower!.Value, newUpper!.Value);

        return true;
    }

    private static List<(ulong, ulong)> GetRanges(string[] input)
    {
        var result = new List<(ulong, ulong)>();

        foreach (var line in input)
        {
            if (line.Contains('-'))
            {
                var tokens = line.Split('-');
                result.Add((ulong.Parse(tokens[0]), ulong.Parse(tokens[1])));
            }
        }

        return result;
    }

    private static List<ulong> GetIngredientIDs(string[] input)
    {
        var result = new List<ulong>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line) || line.Contains('-'))
            {
                continue;
            }

            result.Add(ulong.Parse(line));
        }

        return result;
    }

    private static bool IsInRange((ulong, ulong) range, ulong target)
    {
        return target >= range.Item1 && target <= range.Item2;
    }
}
