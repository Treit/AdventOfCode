using System.Text;
using SuperLinq;
namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 10, CodeType.Original)]
public class Day_10_Original : IPuzzle
{
    private static char[] s_directions = ['U', 'D', 'L', 'R'];

    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        /*
        input = """
        89010123
        78121874
        87430965
        96549874
        45678903
        32019012
        01329801
        10456732
        """.Split(Environment.NewLine);*/
        var trailHeads = GetAllTrailheads(input);
        var summits = GetAllSummits(input);
        var result = 0;

        foreach (var trailhead in trailHeads)
        {
            foreach (var summit in summits)
            {
                var currentPath = new List<Point>();
                var allPaths = new List<List<Point>>();
                var visited = new HashSet<Point>();
                FindPaths(input, trailhead, summit, currentPath, allPaths, visited, true);

                // How many different summits were reachable from this trailhead
                result += allPaths.Count;

                foreach (var path in allPaths)
                {
                    Console.WriteLine($"    ({trailhead.X},{trailhead.Y})->{string.Join("->", path.Select(x => $"({x.X},{x.Y})"))}");
                }
            }
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        return "b";
    }

    private static void FindPaths(
        string[] input,
        Point current,
        Point dest,
        List<Point> currentPath,
        List<List<Point>> paths,
        HashSet<Point> visited,
        bool shortCircuit = false)
    {

        if (paths.Count > 0 && shortCircuit)
        {
            // We only need to find one path and we already have.
            return;
        }

        if (current == dest)
        {
            paths.Add(currentPath.ToList());
            return;
        }

        foreach (var dir in s_directions)
        {
            var next = TryMove(input, current, dir, visited);
            if (next != current)
            {
                visited.Add(next);
                currentPath.Add(next);

                // Recurse
                FindPaths(input, next, dest, currentPath, paths, visited, shortCircuit);

                // Backtrack
                currentPath.Remove(next);
                visited.Remove(next);

            }
        }
    }

    private static Point TryMove(string[] input, Point current, char dir, HashSet<Point> visited)
    {
        var nextPoint = dir switch
        {
            'U' => new Point(current.X - 1, current.Y),
            'D' => new Point(current.X + 1, current.Y),
            'L' => new Point(current.X, current.Y - 1),
            'R' => new Point(current.X, current.Y + 1),
            _ => throw new InvalidOperationException("Unexpected input"),
        };

        if (nextPoint.X >= 0
            && nextPoint.X < input.Length
            && nextPoint.Y >= 0
            && nextPoint.Y < input[nextPoint.X].Length
            && !visited.Contains(nextPoint)
            && char.GetNumericValue(input[nextPoint.X][nextPoint.Y]) == char.GetNumericValue(input[current.X][current.Y]) + 1)
        {
            return nextPoint;
        }

        return current;
    }

    private static List<Point> GetAllTrailheads(string[] input)
    {
        var trailheads = new List<Point>();
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                var height = input[i][j];
                if (height == '0')
                {
                    trailheads.Add(new Point(i, j));
                }
            }
        }

        return trailheads;
    }

    private static List<Point> GetAllSummits(string[] input)
    {
        var summits = new List<Point>();
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                var height = input[i][j];
                if (height == '9')
                {
                    summits.Add(new Point(i, j));
                }
            }
        }

        return summits;
    }

    private record struct Point(int X, int Y);
}
