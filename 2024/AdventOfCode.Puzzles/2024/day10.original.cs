using System.Text;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 10, CodeType.Original)]
public class Day_10_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        input = """
        89010123
        78121874
        87430965
        96549874
        45678903
        32019012
        01329801
        10456732
        """.Split(Environment.NewLine);
        var trailHeads = GetAllTrailheads(input);

        foreach (var head in trailHeads)
        {
            DoWalk('^', head, input, 0);
            DoWalk('>', head, input, 0);
            DoWalk('v', head, input, 0);
            DoWalk('<', head, input, 0);
        }

        return "a";
    }

    private static int DoWalk(char dir, Point start, string[] input, int invalidTries)
    {
        var pos = start;
        var updatedPos = TryMove(pos, dir, input);

        if (updatedPos != pos)
        {
            Console.WriteLine($"Move {dir} from {pos} to {updatedPos} - height is {input[updatedPos.X][updatedPos.Y]}");

            if (input[updatedPos.X][updatedPos.Y] == '9')
            {
                Console.WriteLine("Reached the summit!");
                return 1;
            }

            return DoWalk(dir, updatedPos, input, invalidTries);
        }
        else
        {
            Console.WriteLine($"Can't move {dir} from {pos}");
            var newDir = dir switch
            {
                '^' => '>',
                '>' => 'v',
                'v' => '<',
                '<' => '^',
                _ => throw new InvalidOperationException("Unexpected input")
            };

            invalidTries++;

            if (invalidTries > 4)
            {
                return 0;
            }

            return DoWalk(newDir, pos, input, invalidTries);
        }
    }

    public static string Part2(string[] input)
    {
        return "b";
    }

    private static Point TryMove(Point pos, char dir, string[] input)
    {
        var currVal = char.GetNumericValue(input[pos.X][pos.Y]);
        var (newX, newY) = dir switch
        {
            '^' => (pos.X - 1, pos.Y),
            '>' => (pos.X, pos.Y + 1),
            'v' => (pos.X + 1, pos.Y),
            '<' => (pos.X, pos.Y - 1),
            _ => throw new InvalidOperationException("Unexpected input")
        };

        var canMove =
            newX >= 0 &&
            newX < input.Length &&
            newY >= 0 &&
            newY < input[newX].Length &&
            char.GetNumericValue(input[newX][newY]) == currVal + 1;

        if (canMove)
        {
            return new Point(newX, newY);
        }

        return pos;
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

    record struct Point(int X, int Y);
}
