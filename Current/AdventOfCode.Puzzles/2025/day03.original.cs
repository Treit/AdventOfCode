using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 03, CodeType.Original)]
public class Day_03_Original : IPuzzle
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

        foreach (var line in input)
        {
            var max = -1;

            // First pass. Find the index of the highest first value with characters following it.
            for (var i = 0; i < line.Length - 1; i++)
            {
                var val = char.GetNumericValue(line[i]);
                if (max == -1 || val > char.GetNumericValue(line[max]))
                {
                    max = i;
                }
            }

            // Second pass. Find the highest value after the previously found index.
            var nextMax = -1;
            for (var i = max + 1; i < line.Length; i++)
            {
                var val = char.GetNumericValue(line[i]);
                if (nextMax == -1 || val > char.GetNumericValue(line[nextMax]))
                {
                    nextMax = i;
                }
            }

            var temp = "" + line[max] + line[nextMax];
            answer += int.Parse(temp);

        }

        return answer.ToString();
    }

    public static string Part2(string[] input)
    {
        var answer = 0UL;

        var stack = new Stack<char>();
        foreach (var line in input)
        {
            stack.Clear();

            var toDelete = line.Length - 12;

            foreach (var c in line)
            {
                while (toDelete > 0 && stack.Count > 0 && stack.Peek() < c)
                {
                    _ = stack.Pop();
                    toDelete--;
                }

                stack.Push(c);
            }

            while (toDelete > 0)
            {
                _ = stack.Pop();
                toDelete--;
            }

            var finalValue = 0UL;
            var multiplier = 1UL;

            while (stack.Count > 0)
            {
                var c = stack.Pop();
                finalValue += (ulong)(c - '0') * multiplier;
                multiplier *= 10;
            }

            answer += finalValue;
        }

        return answer.ToString();
    }
}
