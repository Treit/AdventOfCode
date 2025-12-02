using System.Data;
using System.Text;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 05, CodeType.Original)]
public class Day_05_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var (orderings, updates) = ParseData(input);
        var result = 0;

        foreach (var update in updates)
        {
            var valid = true;
            for (var i = 0; i < update.Count; i++)
            {
                for (var j = i + 1; j < update.Count; j++)
                {
                    if (orderings.MustPrecede.TryGetValue(update[i], out var mustPrecede))
                    {
                        if (mustPrecede.Contains(update[j]))
                        {
                            valid = false;
                        }
                    }
                    else if (orderings.MustFollow.TryGetValue(update[j], out var mustFollow))
                    {
                        if (mustFollow.Contains(update[i]))
                        {
                            valid = false;
                        }
                    }
                }
            }

            if (valid)
            {
                result += update[update.Count / 2];
            }
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        var (orderings, updates) = ParseData(input);
        var result = 0;
        var invalidUpdates = new List<List<int>>();
        foreach (var update in updates)
        {
            var valid = true;
            for (var i = 0; i < update.Count; i++)
            {
                for (var j = i + 1; j < update.Count; j++)
                {
                    if (orderings.MustPrecede.TryGetValue(update[i], out var mustPrecede))
                    {
                        if (mustPrecede.Contains(update[j]))
                        {
                            valid = false;
                        }
                    }
                    else if (orderings.MustFollow.TryGetValue(update[j], out var mustFollow))
                    {
                        if (mustFollow.Contains(update[i]))
                        {
                            valid = false;
                        }
                    }
                }
            }

            if (!valid)
            {
                invalidUpdates.Add(update);
            }
        }

        var tmp = 0;
        foreach (var invalidUpdate in invalidUpdates)
        {
            var valid = false;
            while (!valid)
            {
                // LOL 🤪
                for (var i = 0; i < invalidUpdate.Count; i++)
                {
                    valid = true;
                    for (var j = i + 1; j < invalidUpdate.Count; j++)
                    {
                        if (orderings.MustPrecede.TryGetValue(invalidUpdate[i], out var mustPrecede))
                        {
                            if (mustPrecede.Contains(invalidUpdate[j]))
                            {
                                valid = false;
                                tmp = invalidUpdate[i];
                                invalidUpdate[i] = invalidUpdate[j];
                                invalidUpdate[j] = tmp;
                            }
                        }
                        else if (orderings.MustFollow.TryGetValue(invalidUpdate[j], out var mustFollow))
                        {
                            if (mustFollow.Contains(invalidUpdate[i]))
                            {
                                valid = false;
                                tmp = invalidUpdate[j];
                                invalidUpdate[j] = invalidUpdate[i];
                                invalidUpdate[i] = tmp;
                            }
                        }
                    }
                }
            }

            result += invalidUpdate[invalidUpdate.Count / 2];
        }

        return result.ToString();
    }

    private static (Orderings, List<List<int>>) ParseData(string[] input)
    {
        Dictionary<int, HashSet<int>> mustPrecede = [];
        Dictionary<int, HashSet<int>> mustFollow = [];
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

                if (!mustFollow.TryGetValue(first, out value))
                {
                    value = [];
                    mustFollow[first] = value;
                }

                mustFollow[first].Add(second);

                continue;
            }

            updates.Add(line.Split(',').Select(int.Parse).ToList());
        }

        var orderings = new Orderings(mustPrecede, mustFollow);
        return (orderings, updates);
    }

    internal record Orderings(
        Dictionary<int, HashSet<int>> MustPrecede,
        Dictionary<int, HashSet<int>> MustFollow
    );
}
