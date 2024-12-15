namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 06, CodeType.Original)]
public class Day_06_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var re = new Regex(@"(?:turn off|turn on|toggle) (\d+?),(\d+?) through (\d+?),(\d+)$");
        var result = 0;
        var lights = new bool[1000][];
        for (var i = 0; i < 1000; i++)
        {
            lights[i] = new bool[1000];
        }

        foreach (var line in input)
        {
            var m = re.Match(line);
            if (!m.Success)
            {
                throw new InvalidOperationException($"Invalid input: {line}");
            }

            Console.WriteLine(line);
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        var result = 0;
        return result.ToString();
    }
}
