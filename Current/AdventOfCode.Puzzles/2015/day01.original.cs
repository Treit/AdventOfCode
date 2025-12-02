namespace AdventOfCode.Puzzles._2015;

[Puzzle(2015, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Text);
        var part2 = Part2(input.Text);
        return (part1, part2);
    }

    public static string Part1(string input)
    {
        var floor = 0;
        foreach (var c in input)
        {
            if (c == '(') floor++;
            if (c == ')') floor--;
        }

        return floor.ToString();
    }

    public static string Part2(string input)
    {
        var floor = 0;
        var pos = 0;

        foreach (var c in input)
        {
            pos++;
            if (c == '(') floor++;
            if (c == ')') floor--;

            if (floor < 0)
            {
                break;
            }
        }

        return pos.ToString();

    }
}
