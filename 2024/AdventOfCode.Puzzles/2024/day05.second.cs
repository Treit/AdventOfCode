using System.Data;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 05, CodeType.Original)]
public class Day_05_Second : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var (mustPrecedeDict, updates) = ParseData(input);
        var result = 0;

        foreach (var update in updates)
        {
            var valid = IsValid(mustPrecedeDict, update);

            if (valid)
            {
                result += update[update.Count / 2];
            }
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        var (mustPrecedeDict, updates) = ParseData(input);
        var result = 0;
        var invalidUpdates = new List<List<int>>();
        foreach (var update in updates)
        {
            var valid = IsValid(mustPrecedeDict, update);

            if (!valid)
            {
                invalidUpdates.Add(update);
            }
        }

        foreach (var invalidUpdate in invalidUpdates)
        {
            SortUpdate(mustPrecedeDict, invalidUpdate);
            result += invalidUpdate[invalidUpdate.Count / 2];
        }

        return result.ToString();
    }

    private static void SortUpdate(Dictionary<int, HashSet<int>> mustPrecedeDict, List<int> invalidUpdate)
    {
        var valid = false;
        while (!valid)
        {
            for (var i = 0; i < invalidUpdate.Count; i++)
            {
                valid = true;
                for (var j = i + 1; j < invalidUpdate.Count; j++)
                {
                    if (mustPrecedeDict.TryGetValue(invalidUpdate[i], out var mustPrecede))
                    {
                        if (mustPrecede.Contains(invalidUpdate[j]))
                        {
                            valid = false;
                            (invalidUpdate[j], invalidUpdate[i]) = (invalidUpdate[i], invalidUpdate[j]);
                        }
                    }
                }
            }
        }
    }

    private static bool IsValid(Dictionary<int, HashSet<int>> mustPrecedeDict, List<int> update)
    {
        var valid = true;
        for (var i = 0; i < update.Count; i++)
        {
            for (var j = i + 1; j < update.Count; j++)
            {
                if (mustPrecedeDict.TryGetValue(update[i], out var mustPrecede))
                {
                    if (mustPrecede.Contains(update[j]))
                    {
                        valid = false;
                    }
                }
            }
        }

        return valid;
    }

    private static (Dictionary<int, HashSet<int>>, List<List<int>>) ParseData(string[] input)
    {
        Dictionary<int, HashSet<int>> mustPrecede = [];
        var inUpdates = false;
        var updates = new List<List<int>>();

        foreach (var line in input)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                inUpdates = true;
                continue;
            }

            if (!inUpdates)
            {
                var tokens = line.Split('|');
                var first = int.Parse(tokens[0]);
                var second = int.Parse(tokens[1]);

                if (!mustPrecede.TryGetValue(second, out var value))
                {
                    value = [];
                    mustPrecede[second] = value;
                }

                mustPrecede[second].Add(first);

                continue;
            }

            updates.Add(line.Split(',').Select(int.Parse).ToList());
        }

        return (mustPrecede, updates);
    }
}
