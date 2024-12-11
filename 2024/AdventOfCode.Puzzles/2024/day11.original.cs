using System.Diagnostics;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 11, CodeType.Original)]
public class Day_11_Original : IPuzzle
{

    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Text);
        var part2 = Part2(input.Text);
        return (part1, part2);
    }

    public static string Part1(string input)
    {
        var stones = input.Trim().Split(' ').ToList();

        for (var i = 0; i < 25; i++)
        {
            var tmpList = new List<string>();

            foreach (var stone in stones)
            {
                var blinkResult = ProcessBlink(stone);
                tmpList.AddRange(blinkResult);
            }

            stones = tmpList;
        }

        var result = stones.Count;
        return result.ToString();
    }

    public static string Part2(string input)
    {
        var stones = input.Trim().Split(' ').ToList();
        var cache = new Dictionary<string, List<string>>();
        var result = 0L;

        for (var i = 0; i < 75; i++)
        {
            Console.WriteLine(string.Join(",", stones));
            var tmpList = new List<string>();

            foreach (var stone in stones)
            {
                if (!cache.ContainsKey(stone))
                {
                    var blinkResult = ProcessBlink(stone);
                    cache.Add(stone, blinkResult);
                }

                tmpList.AddRange(cache[stone]);
            }

            stones = tmpList;
        }

        return result.ToString();
    }

    private static List<string> ProcessBlink(string stone)
    {
        var result = new List<string>();

        if (stone == "0")
        {
            result.Add("1");
            return result;
        }

        if (stone.Length % 2 == 0)
        {
            var first = long.Parse(stone.Substring(0, stone.Length / 2)).ToString();
            var second = long.Parse(stone.Substring(stone.Length / 2)).ToString();
            result.Add(first);
            result.Add(second);
            return result;
        }

        result.Add((long.Parse(stone) * 2024).ToString());

        return result;
    }
}
