namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 02, CodeType.Original)]
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
        var result = 0L;

        foreach (var str in input)
        {
            var tokens = str.Split('x');
            var dim = new Dims(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
            var surfaceArea = (2 * dim.L * dim.W) + (2 * dim.W * dim.H) + (2 * dim.H * dim.L);
            var sorted = new int[] { dim.L, dim.W, dim.H }.Order().ToArray();
            var smallestSide = sorted[0] * sorted[1];

            result += surfaceArea + smallestSide;
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        var result = 0L;

        foreach (var str in input)
        {
            var tokens = str.Split('x');
            var dim = new Dims(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
            var sorted = new int[] { dim.L, dim.W, dim.H }.Order().ToArray();
            var smallestSide = sorted[0] * sorted[1];

            var volume = dim.L * dim.W * dim.H;
            var perimeter = (sorted[0] * 2) + (sorted[1] * 2);
            result += volume + perimeter;
        }

        return result.ToString();

    }

    private record struct Dims(int L, int W, int H);
}
