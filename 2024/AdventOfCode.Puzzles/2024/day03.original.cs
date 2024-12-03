namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 03, CodeType.Original)]
public partial class Day_03_Original : IPuzzle
{
    [GeneratedRegex(@"(do\(\)|don't\(\))|mul\((\d+?),(\d+?)\)")]
    internal static partial Regex PuzzleRegex();

    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var result = 0UL;
        var re = PuzzleRegex();
        foreach (var match in input.Select(line => re.Matches(line)).SelectMany(match => match).Where(match => match.Value.StartsWith("mul")))
        {
            result += ulong.Parse(match.Groups[2].Value) * ulong.Parse(match.Groups[3].Value);
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        var (ok, result) = (true, 0UL);
        var re = PuzzleRegex();

        foreach (var match in input.Select(line => re.Matches(line)).SelectMany(match => match))
        {
            (ok, result) = match.Value switch
            {
                "do()" => (true, result),
                "don't()" => (false, result),
                _ when ok => (ok, result += ulong.Parse(match.Groups[2].Value) * ulong.Parse(match.Groups[3].Value)),
                _ => (ok, result),
            };
        }

        return result.ToString();
    }
}
