using AoC.Common.Attributes;
using AoC.Common.Interfaces;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AoC.Puzzles._2022.Puzzles;

record struct Sensor((long X, long Y) Position, (long X, long Y) Beacon, long Distance);

[Puzzle(2022, 15, "Beacon Exclusion Zone")]
public class Day15 : IPuzzle<string[]>
{
    static readonly Regex _coordsRegex = new Regex(@"x=([-]*\d+), y=([-]*\d+).+x=([-]*\d+), y=([-]*\d+)");

    public Day15()
    {
    }

    public string[] Parse(string inputText)
    {
        return inputText.Split($"{Environment.NewLine}").Where(x => x.Length > 0).ToArray();
    }

    public string Part1(string[] input)
    {
        var sensors = GetSensors(input);

        foreach (var sensor in sensors)
        {
            Console.WriteLine(sensor);
        }

        (long leftEdge, long rightEdge) = GetRowBoundaries(sensors);

        long rowToCheck = 2_000_000;

        return CoveredCount(sensors, rowToCheck, leftEdge, rightEdge).ToString();
    }

    public string Part2(string[] input)
    {
        var sensors = GetSensors(input);
        long maxRow = 20;
        long minCol = 0;
        long maxCol = 20;

        long iters = 0;

        for (long i = minCol; i < maxCol; i++)
        {
            for (long j = 0; j < maxRow; j++)
            {
                if (iters++ % 1_000_000_000 == 0)
                {
                    Console.Write(".");
                }

                var found = true;
                foreach (var sensor in sensors)
                {
                    if (i == 14 && j == 11)
                    {
                        Debugger.Break();
                    }

                    if (ContainsPoint(sensor, (i, j)))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    Console.WriteLine((i, j));
                    goto Done;
                }
            }
        }

        Done:

        return iters.ToString();
    }

    bool ContainsPoint(Sensor sensor, (long x, long y) point)
    {
        if (point.x < sensor.Position.X - sensor.Distance
            || point.x > sensor.Position.X + sensor.Distance
            || point.y < sensor.Position.Y - sensor.Distance
            || point.y > sensor.Position.Y + sensor.Distance)
        {
            return false;
        }

        return true;
    }

    long ManhattanDistance((long x, long y) a, (long x, long y) b)
    {
        return Math.Abs((b.x - a.x)) + Math.Abs((b.y - a.y));
    }

    (long LeftEdge, long RightEdge) GetRowBoundaries(IEnumerable<Sensor> sensors)
    {
        long smallestPosX = sensors.Min(x => x.Position.X);
        long smallestBeaconX = sensors.Min(x => x.Beacon.X);
        long smallestX = Math.Min(smallestPosX, smallestBeaconX);
        long largestPosX = sensors.Max(x => x.Position.X);
        long largestBeaconX = sensors.Max(x => x.Beacon.X);
        long largestX = Math.Max(largestPosX, largestBeaconX);
        long longestDistance = sensors.Max(x => x.Distance);

        long minX = smallestX - longestDistance;
        long maxX = largestX + longestDistance;

        return (minX, maxX);
    }

    long CoveredCount(IEnumerable<Sensor> sensors, long targetRow, long leftEdge, long rightEdge)
    {
        var covered = new HashSet<(long, long)>();

        foreach (var sensor in sensors)
        {
            for (long i = leftEdge; i < rightEdge; i++)
            {
                var cell = (x: i, y: targetRow);

                if (cell == sensor.Beacon)
                {
                    Console.WriteLine($"{cell} has a beacon.");
                    continue;
                }

                var dist = ManhattanDistance(sensor.Position, cell);
                if (dist <= sensor.Distance)
                {
                    covered.Add(cell);
                }
            }
        }

        return covered.Count;
    }

    HashSet<(long, long)> NotCovered(
        IEnumerable<Sensor> sensors,
        long targetRow,
        long leftEdge,
        long rightEdge)
    {
        var notcovered = new HashSet<(long, long)>();

        for (long i = leftEdge; i <= rightEdge; i++)
        {
            var cell = (x: i, y: targetRow);

            bool covered = false;
            foreach (var sensor in sensors)
            {
                var dist = ManhattanDistance(sensor.Position, cell);
                if (dist <= sensor.Distance)
                {
                    covered = true;
                    break;
                }
            }

            if (!covered)
            {
                notcovered.Add(cell);
            }
        }

        return notcovered;
    }

    long Smallest(params long[] values)
    {
        return values.Min();
    }

    long Largest(params long[] values)
    {
        return values.Max();
    }

    List<Sensor> GetSensors(string[] input)
    {
        var sensors = new List<Sensor>(input.Length);

        foreach (var line in input)
        {
            var m = _coordsRegex.Match(line);

            if (!m.Success)
            {
                continue;
            }

            (long x, long y) pos = (long.Parse(m.Result("$1")), long.Parse(m.Result("$2")));
            var beacon = (long.Parse(m.Result("$3")), long.Parse(m.Result("$4")));
            var dist = ManhattanDistance(pos, beacon);
            var sensor = new Sensor(pos, beacon, dist);

            sensors.Add(sensor);
        }

        return sensors;
    }
}