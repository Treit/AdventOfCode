using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 04, CodeType.Original)]
public class Day_04_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var answer = 0;

        /*
        input = """
        ..@@.@@@@.
        @@@.@.@.@@
        @@@@@.@.@@
        @.@@@@..@.
        @@.@@@@.@@
        .@@@@@@@.@
        .@.@.@.@@@
        @.@@@.@@@@
        .@@@@@@@@.
        @.@.@@@.@.
        """.Split(Environment.NewLine);
        */

        var grid = GetGrid(input);

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[i].Length; j++)
            {
                if (grid[i][j] != '@')
                {
                    continue;
                }

                var adjacentCount = CountNeighboringCells(grid, i, j, '@');
                if (adjacentCount < 4)
                {
                    answer++;
                }
            }
        }

        return answer.ToString();
    }

    public static string Part2(string[] input)
    {
        var answer = 0;

        /*
        input = """
        ..@@.@@@@.
        @@@.@.@.@@
        @@@@@.@.@@
        @.@@@@..@.
        @@.@@@@.@@
        .@@@@@@@.@
        .@.@.@.@@@
        @.@@@.@@@@
        .@@@@@@@@.
        @.@.@@@.@.
        """.Split(Environment.NewLine);
        */

        var grid = GetGrid(input);
        var totalRemoved = 0;

        while (true)
        {
            // PrintGrid(grid);

            var removedCount = 0;

            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] != '@')
                    {
                        continue;
                    }

                    var adjacentCount = CountNeighboringCells(grid, i, j, '@');
                    if (adjacentCount < 4)
                    {
                        grid[i][j] = 'x';
                        removedCount++;
                    }
                }
            }

            totalRemoved += removedCount;

            Console.WriteLine($"Removed in this pass: {removedCount}. Total removed: {totalRemoved}");

            if (removedCount == 0)
            {
                break;
            }
        }

        answer = totalRemoved;

        return answer.ToString();
    }

    private static char[][] GetGrid(string[] input)
    {
        var result = new char[input.Length][];

        for (var i = 0; i < input.Length; i++)
        {
            result[i] = input[i].ToCharArray();
        }

        return result;
    }

    private static void PrintGrid(char[][] grid)
    {
        Console.WriteLine();

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid.Length; j++)
            {
                Console.Write(grid[i][j]);
            }

            Console.WriteLine();
        }
    }

    private static int CountNeighboringCells(
        char[][] grid,
        int row,
        int col,
        char target)
    {
        var count = 0;

        // R
        if (GetValue(grid, row, col + 1) == target)
        {
            count++;
        }

        // DR
        if (GetValue(grid, row + 1, col + 1) == target)
        {
            count++;
        }

        // D
        if (GetValue(grid, row + 1, col) == target)
        {
            count++;
        }

        // DL
        if (GetValue(grid, row + 1, col - 1) == target)
        {
            count++;
        }

        // L
        if (GetValue(grid, row, col - 1) == target)
        {
            count++;
        }

        // UL
        if (GetValue(grid, row - 1, col - 1) == target)
        {
            count++;
        }

        // U
        if (GetValue(grid, row - 1, col) == target)
        {
            count++;
        }

        // UR
        if (GetValue(grid, row - 1, col + 1) == target)
        {
            count++;
        }

        return count;
    }

    private static char? GetValue(char[][] grid, int targetRow, int targetCol)
    {
        if (targetRow >= 0 && targetRow < grid.Length && targetCol >= 0 && targetCol < grid[targetRow].Length)
        {
            return grid[targetRow][targetCol];
        }

        return null;

    }
}
