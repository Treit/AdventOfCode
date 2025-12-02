using System.Data;
using System.Text;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 06, CodeType.Original)]
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
        var visited = DoWalk(input);
        return visited.Count.ToString();
    }

    public static string Part2(string[] input)
    {
        // Initial walk to get all possible positions to block
        var startPos = GetStartingPosition(input);
        var visited = DoWalk(input);
        var result = 0;

        // Now try blocking each possible position and see if we create a loop
        foreach (var kvp in visited)
        {
            var pos = kvp.Key;
            if (pos == startPos)
            {
                continue;
            }

            var newVisited = DoWalk(input, pos);
            if (newVisited.Values.Any(x => x > 5))
            {
                // Loop!
                result++;
            }
        }

        return result.ToString();
    }

    private static Dictionary<(int, int), int> DoWalk(string[] input, (int, int)? blockPos = null)
    {
        var pos = GetStartingPosition(input);
        var currentDir = '^';
        var visited = new Dictionary<(int, int), int>
        {
            { pos, 1 }
        };

        while (true)
        {
            var (newRow, newCol) = Advance(pos.row, pos.col, currentDir, input);
            if (newRow == pos.row && newCol == pos.col)
            {
                // Blocked
                currentDir = TurnRight(currentDir);
            }
            else if (blockPos != null && newRow == blockPos.Value.Item1 && newCol == blockPos.Value.Item2)
            {
                // Blocked
                currentDir = TurnRight(currentDir);
            }
            else if (newRow == -1 && newCol == -1)
            {
                // Left the area
                break;
            }
            else
            {
                // Advance
                pos = (newRow, newCol);
                if (!visited.TryGetValue(pos, out var existing))
                {
                    visited.Add(pos, 1);
                }
                else
                {
                    visited[pos] = existing + 1;
                    if (visited[pos] > 5)
                    {
                        // Heuristic: we are in a loop
                        break;
                    }
                }
            }
        }

        return visited;

    }

    private static (int row, int col) Advance(int row, int col, char currDir, string[] input)
    {
        var (newRow, newCol) = currDir switch
        {
            '^' => (row - 1, col),
            '>' => (row, col + 1),
            'v' => (row + 1, col),
            '<' => (row, col - 1),
            _ => throw new InvalidOperationException("Unexpected input"),
        };

        if (newRow < 0 ||
            newRow >= input.Length ||
            newCol < 0 ||
            newCol >= input[row].Length)
        {
            // Escaped the area
            return (-1, -1);
        }

        if (input[newRow][newCol] is '#' or 'O')
        {
            // Blocked
            return (row, col);
        }

        return (newRow, newCol);
    }

    private static char TurnRight(char current)
    {
        return current switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            _ => throw new InvalidOperationException("Unexpected input"),
        };
    }

    private static (int row, int col) GetStartingPosition(string[] input)
    {
        for (var row = 0; row < input.Length; row++)
        {
            for (var col = 0; col < input[row].Length; col++)
            {
                if (input[row][col] == '^')
                {
                    return (row, col);
                }
            }
        }

        return (-1, -1);
    }
}
