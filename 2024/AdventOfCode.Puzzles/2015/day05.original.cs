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
                result++;
            }
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        var result = 0;

        foreach (var inputLine in input)
        {
            var line = inputLine.Trim();

            var pairs = GeneratePairs(line).ToArray();
            var normalizedPairs = new List<string>(pairs.Length);

            var prev = "";
            foreach (var pair in pairs)
            {
                if (pair == prev)
                {
                    continue;
                }

                normalizedPairs.Add(pair);

                prev = pair;
            }

            var counts = normalizedPairs.CountBy(x => x);
            Console.WriteLine(line);
            var triples = GenerateTriples(line);
            var good = false;

            foreach (var triple in triples)
            {
                if (triple[0] == triple[2])
                {
                    good = true;
                }
            }

            if (counts.Any(x => x.Value >= 2) && good)
            {
                result++;
            }
        }

        return result.ToString();
    }

    private static IEnumerable<string> GeneratePairs(string input)
    {
        var ptrA = 0;

        while (ptrA < input.Length - 1)
        {
            var slice = input.AsSpan(ptrA, 2);
            var tmp = new string(slice);
            yield return tmp;
            ptrA++;
        }
    }

    private static IEnumerable<string> GenerateTriples(string input)
    {
        var ptrA = 0;

        while (ptrA < input.Length - 2)
        {
            var slice = input.AsSpan(ptrA, 3);
            var tmp = new string(slice);
            yield return tmp;
            ptrA++;
        }
    }
}
