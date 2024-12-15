var lights = GetLightGrid(1000, 1000);
var actions = Parse(File.ReadAllLines("input.txt"));

foreach (var action in actions)
{
    ProcessAction(action, lights);
    Console.WriteLine($"💡 -> {CountOfLightsOn(lights)}");
}

var count = CountOfLightsOn(lights);

Console.WriteLine(count);

static void ProcessAction(LightAction action, bool[][] lights)
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

static List<LightAction> Parse(string[] input)
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

static int CountOfLightsOn(bool[][] lights)
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

static bool[][] GetLightGrid(int rows, int cols)
{
    var lights = new bool[rows][];
    for (var i = 0; i < cols; i++)
    {
        lights[i] = new bool[cols];
    }

    return lights;
}

internal record LightAction(
    string Op,
    int StartX,
    int StartY,
    int EndX,
    int EndY);
