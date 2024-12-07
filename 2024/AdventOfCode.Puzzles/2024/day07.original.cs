using System.Data;
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
        foreach (var line in input)
        {
            Console.WriteLine(line);
        }
        return "foo";
    }

    public static string Part2(string[] input)
    {
        return "bar";
    }
}
