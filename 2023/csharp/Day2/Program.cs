using System.Diagnostics;
using System.Text.RegularExpressions;

var inputfile = args.Any(arg => arg.Contains("test")) ? "testinput.txt" : "input.txt";
var input = File.ReadAllLines(inputfile);

if (args.Any(arg => arg.Contains("2")))
{
    Part2(input);
    return;
}

Part1(input);

void Part1(string[] input)
{
    var maxRed = 12;
    var maxGreen = 13;
    var maxBlue = 14;
    var answer = 0;

    foreach (var line in input)
    {
        var normalized = line.Replace(" ", "");
        var tokens = normalized.Split(":");
        var gameId = int.Parse(tokens[0].Replace("Game", ""));
        var results = tokens[1].Replace(";", ",");
        tokens = results.Split(",");
        var valid = true;

        foreach (var token in tokens)
        {
            var match = Regex.Match(token, @"(\d+)(\w+)");
            var num = int.Parse(match.Result("$1"));
            var color = match.Result("$2");
            if (color == "green" && num > maxGreen) valid = false;
            if (color == "red" && num > maxRed) valid = false;
            if (color == "blue" && num > maxBlue) valid = false;
        }

        if (valid)
        {
            answer += gameId;
        }
    }

    Console.WriteLine(answer);
}

void Part2(string[] input)
{
    var answer = 0;

    foreach (var line in input)
    {
        var normalized = line.Replace(" ", "");
        var tokens = normalized.Split(":");
        var gameId = int.Parse(tokens[0].Replace("Game", ""));
        var results = tokens[1].Replace(";", ",");
        tokens = results.Split(",");
        var maxGreenSeen = 0;
        var maxRedSeen = 0;
        var maxBlueSeen = 0;

        foreach (var token in tokens)
        {
            var match = Regex.Match(token, @"(\d+)(\w+)");
            var num = int.Parse(match.Result("$1"));
            var color = match.Result("$2");
            if (color == "green" && num > maxGreenSeen) maxGreenSeen = num;
            if (color == "red" && num > maxRedSeen) maxRedSeen = num;
            if (color == "blue" && num > maxBlueSeen) maxBlueSeen = num;
        }

        var power = maxGreenSeen * maxRedSeen * maxBlueSeen;
        answer += power;
    }

    Console.WriteLine(answer);
}
