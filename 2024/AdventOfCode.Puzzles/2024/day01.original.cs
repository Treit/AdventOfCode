namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 01, CodeType.Original)]
public class Day_01_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);

        var part2 = Part2(input.Lines);

        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var (firstList, secondList) = GetLists(input);
        firstList.Sort();
        secondList.Sort();
        var pairWise = firstList.Zip(secondList);
        long totalDistance = pairWise.Select((tup) => Math.Abs(tup.Second - tup.First)).Sum();

        return totalDistance.ToString();
    }

    public static string Part2(string[] input)
    {
        var (firstList, secondList) = GetLists(input);

        var grouped = secondList.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        var totalSimilarityScore = firstList.Select(x => x * (grouped.TryGetValue(x, out var y) ? y : 0)).Sum();

        return totalSimilarityScore.ToString();
    }

    private static (List<int> firstList, List<int> secondList) GetLists(string[] input)
    {
        var firstList = new List<int>(input.Length);
        var secondList = new List<int>(input.Length);
        var re = new Regex(@"(\d+)\s+(\d+)$");
        foreach (var line in input)
        {
            var match = re.Match(line);
            var first = int.Parse(match.Groups[1].Value);
            var second = int.Parse(match.Groups[2].Value);
            firstList.Add(first);
            secondList.Add(second);
        }

        return (firstList, secondList);
    }
}
