using System.Runtime.InteropServices;
using System.Transactions;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 02, CodeType.Original)]
public class Day_02_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var ids = input[0].Split(",");
        var answer = 0L;

        foreach (var id in ids)
        {
            var parts = id.Split("-");
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);

            for (var i = start; i <= end; i++)
            {
                if (IsInvalid(i))
                {
                    answer += i;
                }
            }
        }

        return answer.ToString();
    }

    public static string Part2(string[] input)
    {
        var ids = input[0].Split(",");
        var answer = 0L;
        foreach (var id in ids)
        {
            var parts = id.Split("-");
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);

            for (var i = start; i <= end; i++)
            {
                if (IsInvalid2(i))
                {
                    answer += i;
                }
            }
        }

        return answer.ToString();

    }

    private static bool IsInvalid(long value)
    {
        var tmp = value.ToString();
        var len = tmp.Length / 2;
        var first = tmp.Substring(0, len);
        var second = tmp.Substring(len);

        if (first == second)
        {
            return true;
        }

        return false;
    }

    private static bool IsInvalid2(long value)
    {
        var tmp = value.ToString();

        foreach (var substr in Substrings(tmp))
        {
            if (substr.Length == tmp.Length)
            {
                continue;
            }

            var groups = tmp.Length / substr.Length;
            var candidate = string.Empty;

            for (var i = 0; i < groups; i++)
            {
                candidate += substr;
            }

            if (tmp == candidate)
            {
                return true;
            }
        }

        return false;
    }

    private static IEnumerable<string> Substrings(string source)
    {
        for (int i = 0; i < source.Length; i++)
        {
            string substring = string.Empty;

            for (int j = i; j < source.Length; j++)
            {
                substring += source[j];
                yield return substring;
            }
        }

    }
}
