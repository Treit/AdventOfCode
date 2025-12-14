using System.Threading;

namespace AdventOfCode.Puzzles._2024;

internal record struct Point(int X, int Y);

[Puzzle(2024, 08, CodeType.Original)]
public class Day_08_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        /*
        input = """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """.Split(Environment.NewLine);
        */

        var antennaLocations = new Dictionary<char, List<Point>>();
        var antiNodes = new HashSet<Point>();

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                var c = input[i][j];
                if (c == '.')
                {
                    continue;
                }

                if (!antennaLocations.TryGetValue(c, out var locs))
                {
                    antennaLocations.Add(c, []);
                }

                antennaLocations[c].Add(new Point(i, j));
            }
        }

        foreach (var kvp in antennaLocations)
        {
            var current = kvp.Key;
            var locs = kvp.Value;

            foreach (var loc in locs)
            {
                foreach (var otherLoc in locs)
                {
                    if (otherLoc == loc)
                    {
                        continue;
                    }

                    var xDiff = Math.Abs(loc.X - otherLoc.X);
                    var yDiff = Math.Abs(loc.Y - otherLoc.Y);
                    Point antiNode = loc;

                    if (loc.X < otherLoc.X && loc.Y < otherLoc.Y)
                    {
                        antiNode = new Point(loc.X - xDiff, loc.Y - yDiff);
                    }
                    else if (loc.X < otherLoc.X && loc.Y > otherLoc.Y)
                    {
                        antiNode = new Point(loc.X - xDiff, loc.Y + yDiff);
                    }
                    else if (loc.X > otherLoc.X && loc.Y < otherLoc.Y)
                    {
                        antiNode = new Point(loc.X + xDiff, loc.Y - yDiff);
                    }
                    else
                    {
                        antiNode = new Point(loc.X + xDiff, loc.Y + yDiff);
                    }

                    if (antiNode.X >= 0 && antiNode.X < input.Length && antiNode.Y >= 0 && antiNode.Y < input[0].Length)
                    {
                        antiNodes.Add(antiNode);
                    }
                }
            }
        }

        return antiNodes.Count.ToString();
    }

    public static string Part2(string[] input)
    {
        /*
        input = """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """.Split(Environment.NewLine);*/

        var antennaLocations = new Dictionary<char, List<Point>>();
        var antiNodes = new HashSet<Point>();

        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[0].Length; j++)
            {
                var c = input[i][j];
                if (c == '.')
                {
                    continue;
                }

                if (!antennaLocations.TryGetValue(c, out var locs))
                {
                    antennaLocations.Add(c, []);
                }

                antennaLocations[c].Add(new Point(i, j));
            }
        }

        foreach (var kvp in antennaLocations)
        {
            var current = kvp.Key;
            var locs = kvp.Value;

            foreach (var loc in locs)
            {
                foreach (var otherLoc in locs)
                {
                    if (otherLoc == loc)
                    {
                        continue;
                    }

                    var locX = loc.X;
                    var otherLocX = otherLoc.X;
                    var locY = loc.Y;
                    var otherLocY = otherLoc.Y;
                    var xDiff = Math.Abs(locX - otherLocX);
                    var yDiff = Math.Abs(locY - otherLocY);

                    var dir = "";
                    if (locX < otherLocX && locY < otherLocY)
                    {
                        dir = @"\";
                    }
                    else if (locX < otherLocX && locY > otherLocY)
                    {
                        dir = "/";
                    }
                    else if (locX > otherLocX && locY < otherLocY)
                    {
                        dir = "/";
                    }
                    else
                    {
                        dir = @"\";
                    }

                    Console.WriteLine($"Placing antinodes along the {dir} diagonal using {xDiff} up / down and {yDiff} left / right ({loc}, {otherLoc})");

                    if (dir == "/")
                    {
                        var currX = locX;
                        var currY = locY;
                        while (true)
                        {
                            // Go up right
                            var antiNode = new Point(currX - xDiff, currY + yDiff);
                            if (antiNode.X >= 0 && antiNode.X < input.Length && antiNode.Y >= 0 && antiNode.Y < input[0].Length)
                            {
                                antiNodes.Add(antiNode);
                                currX = antiNode.X;
                                currY = antiNode.Y;
                            }
                            else
                            {
                                break;
                            }
                        }

                        while (true)
                        {
                            // Go down left
                            var antiNode = new Point(currX + xDiff, currY - yDiff);
                            if (antiNode.X >= 0 && antiNode.X < input.Length && antiNode.Y >= 0 && antiNode.Y < input[0].Length)
                            {
                                antiNodes.Add(antiNode);
                                currX = antiNode.X;
                                currY = antiNode.Y;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (dir == @"\")
                    {
                        var currX = locX;
                        var currY = locY;
                        while (true)
                        {
                            // Go up left
                            var antiNode = new Point(currX - xDiff, currY - yDiff);
                            if (antiNode.X >= 0 && antiNode.X < input.Length && antiNode.Y >= 0 && antiNode.Y < input[0].Length)
                            {
                                antiNodes.Add(antiNode);
                                currX = antiNode.X;
                                currY = antiNode.Y;
                            }
                            else
                            {
                                break;
                            }
                        }

                        while (true)
                        {
                            // Go down right
                            var antiNode = new Point(currX + xDiff, currY + yDiff);
                            if (antiNode.X >= 0 && antiNode.X < input.Length && antiNode.Y >= 0 && antiNode.Y < input[0].Length)
                            {
                                antiNodes.Add(antiNode);
                                currX = antiNode.X;
                                currY = antiNode.Y;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        return antiNodes.Count.ToString();
    }
}
