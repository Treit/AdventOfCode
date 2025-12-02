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
        var lights = GetLightGrid(1000, 1000);
        var actions = Parse(input);

        foreach (var action in actions)
        {
            ProcessAction(action, lights);
        }

        var count = CountOfLightsOn(lights);
        return count.ToString();

    }

    public static string Part2(string[] input)
    {
        var brightLights = GetLightBrightnessGrid(1000, 1000);
        var actions = Parse(input);

        foreach (var action in actions)
        {
            ProcessBrightnessAction(action, brightLights);
        }

        var brightness = GetTotalBrightness(brightLights);
        return brightness.ToString();
    }

    private static void ProcessBrightnessAction(LightAction action, ulong[][] lights)
    {
        for (var i = action.StartX; i <= action.EndX; i++)
        {
            for (var j = action.StartY; j <= action.EndY; j++)
            {
                ref var target = ref lights[i][j];
                if (action.Op == "on")
                {
                    target += 1;
                }
                else if (action.Op == "off" && target > 0)
                {
                    target -= 1;
                }
                else if (action.Op == "flip")
                {
                    target += 2;
                }
            }
        }
    }

    private static void ProcessAction(LightAction action, bool[][] lights)
    {
        for (var i = action.StartX; i <= action.EndX; i++)
        {
            for (var j = action.StartY; j <= action.EndY; j++)
            {
                lights[i][j] = (action.Op, lights[i][j]) switch
                {
                    ("on", _) => true,
                    ("off", _) => false,
                    ("flip", true) => false,
                    ("flip", false) => true,
                    _ => throw new InvalidOperationException("Unexpected state"),
                };
            }
        }
    }

    private static List<LightAction> Parse(string[] input)
    {
        var result = new List<LightAction>();
        var re = new Regex(@"turn off|turn on|toggle|(\d+),(\d+) through (\d+),(\d+)$");

        foreach (var line in input)
        {
            var matches = re.Matches(line);
            if (matches.Count != 2)
            {
                throw new InvalidOperationException($"Unexpected input: {line}");
            }

            var op = matches[0].Value switch
            {
                "turn off" => "off",
                "turn on" => "on",
                "toggle" => "flip",
                _ => throw new InvalidOperationException($"Unexpected input: {matches[0].Value}"),
            };

            var startX = int.Parse(matches[1].Groups[1].Value);
            var startY = int.Parse(matches[1].Groups[2].Value);
            var endX = int.Parse(matches[1].Groups[3].Value);
            var endY = int.Parse(matches[1].Groups[4].Value);

            result.Add(new LightAction(op, startX, startY, endX, endY));
        }

        return result;
    }

    private static int CountOfLightsOn(bool[][] lights)
    {
        var count = 0;

        for (var i = 0; i < lights.Length; i++)
        {
            for (var j = 0; j < lights[i].Length; j++)
            {
                if (lights[i][j])
                {
                    count++;
                }
            }
        }

        return count;
    }

    private static bool[][] GetLightGrid(int rows, int cols)
    {
        var lights = new bool[rows][];
        for (var i = 0; i < cols; i++)
        {
            lights[i] = new bool[cols];
        }

        return lights;
    }


    private static ulong[][] GetLightBrightnessGrid(int rows, int cols)
    {
        var lights = new ulong[rows][];
        for (var i = 0; i < cols; i++)
        {
            lights[i] = new ulong[cols];
        }

        return lights;
    }

    private static ulong GetTotalBrightness(ulong[][] lights)
    {
        var total = 0UL;

        for (var i = 0; i < lights.Length; i++)
        {
            for (var j = 0; j < lights[i].Length; j++)
            {
                total += lights[i][j];
            }
        }

        return total;
    }


    internal record LightAction(
        string Op,
        int StartX,
        int StartY,
        int EndX,
        int EndY);

}
