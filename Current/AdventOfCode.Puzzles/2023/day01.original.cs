using System.Diagnostics;

namespace AdventOfCode.Puzzles._2023;

[Puzzle(2023, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);

        var part2 = Part2(input.Lines);

        return (part1, part2);
    }

    private static string Part1(string[] input)
    {
        // Part 1
        var sum = 0;
        foreach (var line in input)
        {
            var first = "";
            var last = "";

            foreach (char c in line)
            {
                if (!char.IsDigit(c))
                {
                    continue;
                }

                if (first.Length == 0)
                {
                    first = c.ToString();
                }

                last = c.ToString();
            }

            sum += int.Parse(first + last);
        }

        return sum.ToString();
    }

    private static string Part2(string[] input)
    {
        var sum = 0;
        foreach (var tmp in input)
        {
            var first = "";
            var last = "";

            for (var i = 0; i < tmp.Length; i++)
            {
                var digit = DigitAtCurrent(tmp, i);

                if (digit is null)
                {
                    continue;
                }

                if (first.Length == 0)
                {
                    first = digit;
                }

                last = digit;
            }

            sum += int.Parse(first + last);

        }

        return sum.ToString();
    }

    private static string? DigitAtCurrent(string input, int pos)
    {
        if (char.IsDigit(input[pos]))
        {
            return input[pos].ToString();
        }

        if (input.Substring(pos, Math.Min("one".Length, input.Length - pos)) == "one") return "1";
        if (input.Substring(pos, Math.Min("two".Length, input.Length - pos)) == "two") return "2";
        if (input.Substring(pos, Math.Min("three".Length, input.Length - pos)) == "three") return "3";
        if (input.Substring(pos, Math.Min("four".Length, input.Length - pos)) == "four") return "4";
        if (input.Substring(pos, Math.Min("five".Length, input.Length - pos)) == "five") return "5";
        if (input.Substring(pos, Math.Min("six".Length, input.Length - pos)) == "six") return "6";
        if (input.Substring(pos, Math.Min("seven".Length, input.Length - pos)) == "seven") return "7";
        if (input.Substring(pos, Math.Min("eight".Length, input.Length - pos)) == "eight") return "8";
        if (input.Substring(pos, Math.Min("nine".Length, input.Length - pos)) == "nine") return "9";

        return null;
    }
}
