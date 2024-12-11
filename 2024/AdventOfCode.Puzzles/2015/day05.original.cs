using System.Security.Cryptography;
using System.Text;

namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 05, CodeType.Original)]
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
        var result = 0;

        foreach (var line in input)
        {
            var vc = 0;

            if (line.Contains("ab")
                || line.Contains("cd")
                || line.Contains("pq")
                || line.Contains("xy"))
            {
                Console.WriteLine($"{line} is naughty");
                continue;
            }

            var prev = '!';
            var consec = false;

            foreach (var c in line)
            {
                if (c == prev)
                {
                    consec = true;
                }

                if (c is 'a' or 'e' or 'i' or 'o' or 'u')
                {
                    vc++;
                }

                prev = c;
            }

            if (consec && vc >= 3)
            {
                Console.WriteLine($"{line} is nice");
                result++;
            }
            else
            {
                Console.WriteLine($"{line} is naughty");
            }
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        return "";
    }
}
