namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 03, CodeType.Original)]
public class Day_03_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var re = new Regex(@"mul\((\d+?),(\d+?)\)");
        var result = 0UL;

        foreach (var line in input)
        {
            var matches = re.Matches(line);
            foreach (Match match in matches)
            {
                var numA = ulong.Parse(match.Groups[1].Value);
                var numB = ulong.Parse(match.Groups[2].Value);
                result += numA * numB;
            }
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        var re = new Regex(@"(do\(\)|don't\(\))|mul\((\d+?),(\d+?)\)");
        var result = 0UL;

        var ok = true;
        foreach (var line in input)
        {
            Console.WriteLine(input);
            var matches = re.Matches(line);
            foreach (Match match in matches)
            {
                if (match.Value == "do()")
                {
                    ok = true;
                }
                else if (match.Value == "don't()")
                {
                    ok = false;
                }
                else
                {
                    var numA = ulong.Parse(match.Groups[2].Value);
                    var numB = ulong.Parse(match.Groups[3].Value);

                    if (ok)
                    {
                        result += numA * numB;
                    }
                }
            }
        }

        return result.ToString();

    }
}
