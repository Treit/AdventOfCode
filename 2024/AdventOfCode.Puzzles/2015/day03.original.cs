namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 03, CodeType.Original)]
public class Day_03_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Text);
        var part2 = Part2(input.Text);
        return (part1, part2);
    }

    public static string Part1(string input)
    {
        var results = new Dictionary<Point, int>();
        var currPos = new Point(0, 0);
        results.Add(currPos, 1);

        foreach (var dir in input)
        {
            var newPos = dir switch
            {
                '^' => new Point(currPos.X - 1, currPos.Y),
                '>' => new Point(currPos.X, currPos.Y + 1),
                'v' => new Point(currPos.X + 1, currPos.Y),
                '<' => new Point(currPos.X, currPos.Y - 1),
                _ => throw new InvalidOperationException("Unexpected input")
            };

            if (!results.TryGetValue(newPos, out var existing))
            {
                results.Add(newPos, 1);
            }
            else
            {
                results[newPos] = existing + 1;
            }

            currPos = newPos;
        }

        return results.Count.ToString();
    }

    public static string Part2(string input)
    {
        var results = new Dictionary<Point, int>();
        var posA = new Point(0, 0);
        var posB = posA;
        results.Add(posA, 2);
        var targetPos = posB;
        var num = 0;

        foreach (var dir in input)
        {
            targetPos = num % 2 == 0 ? posA : posB;
            targetPos = dir switch
            {
                '^' => new Point(targetPos.X - 1, targetPos.Y),
                '>' => new Point(targetPos.X, targetPos.Y + 1),
                'v' => new Point(targetPos.X + 1, targetPos.Y),
                '<' => new Point(targetPos.X, targetPos.Y - 1),
                _ => throw new InvalidOperationException("Unexpected input")
            };

            if (!results.TryGetValue(targetPos, out var existing))
            {
                results.Add(targetPos, 1);
            }
            else
            {
                results[targetPos] = existing + 1;
            }

            if (num++ % 2 == 0)
            {
                posA = targetPos;
            }
            else
            {
                posB = targetPos;
            }
        }

        return results.Count.ToString();
    }

    private record struct Point(int X, int Y);
}
