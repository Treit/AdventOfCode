using System.Text;

namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 04, CodeType.Original)]
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
        var cols = input[0].Length;
        var rows = input.Length;
        var result = 0;

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                var results = DoWalk(row, col, 4, input).Where(x => x == "XMAS");
                result += results.Count();
            }
        }

        return result.ToString();
    }

    public static string Part2(string[] input)
    {
        var cols = input[0].Length;
        var rows = input.Length;
        var result = 0;

        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                var temp = new string(DoWalk2(row, col, 4, input));
                if (temp is "AMMSS" or "AMSMS" or "ASMSM" or "ASSMM")
                {
                    result++;
                }
            }
        }

        return result.ToString();
    }

    public static IEnumerable<string> DoWalk(int startRow, int startCol, int length, string[] input)
    {
        // Walk from the current position in all 8 possible directions
        // (Up, Down, Left, Right, Diagonal Up Right, Diagonal Up Left,
        // Diagonal Down Right, Diagonal Down Left
        var chars = new List<char>();

        // Up
        var sb = new StringBuilder();
        var row = startRow;
        var col = startCol;

        for (var i = 0; i < length; i++)
        {
            var c = CharAt(input, row, col);
            sb.Append(c);
            row--;
        }

        yield return sb.ToString();

        // Down
        sb.Clear();
        row = startRow;
        col = startCol;
        for (var i = 0; i < length; i++)
        {
            var c = CharAt(input, row, col);
            sb.Append(c);
            row++;
        }

        yield return sb.ToString();

        // Left
        sb.Clear();
        row = startRow;
        col = startCol;

        for (var i = 0; i < length; i++)
        {
            var c = CharAt(input, row, col);
            sb.Append(c);
            col--;
        }

        yield return sb.ToString();

        // Right
        sb.Clear();
        row = startRow;
        col = startCol;

        for (var i = 0; i < length; i++)
        {
            var c = CharAt(input, row, col);
            sb.Append(c);
            col++;
        }

        yield return sb.ToString();

        // Diagonal Up Left
        sb.Clear();
        row = startRow;
        col = startCol;

        for (var i = 0; i < length; i++)
        {
            var c = CharAt(input, row, col);
            sb.Append(c);
            row--;
            col--;
        }

        yield return sb.ToString();

        // Diagonal Up Right
        sb.Clear();
        row = startRow;
        col = startCol;

        for (var i = 0; i < length; i++)
        {
            var c = CharAt(input, row, col);
            sb.Append(c);
            row--;
            col++;
        }

        yield return sb.ToString();

        // Diagonal Down Left
        sb.Clear();
        row = startRow;
        col = startCol;

        for (var i = 0; i < length; i++)
        {
            var c = CharAt(input, row, col);
            sb.Append(c);
            row++;
            col--;
        }

        yield return sb.ToString();

        // Diagonal Down Right
        sb.Clear();
        row = startRow;
        col = startCol;

        for (var i = 0; i < length; i++)
        {
            var c = CharAt(input, row, col);
            sb.Append(c);
            row++;
            col++;
        }

        yield return sb.ToString();
    }

    public static string DoWalk2(int startRow, int startCol, int length, string[] input)
    {
        // From the current position (the middle of the X) walk Up Left, up Right, down Left, down Right
        // and build the resulting string.
        var chars = new List<char>();

        var sb = new StringBuilder();
        var row = startRow;
        var col = startCol;

        // Center of X
        sb.Append(CharAt(input, row, col));

        // Up Left
        sb.Append(CharAt(input, row - 1, col - 1));
        // Up Right
        sb.Append(CharAt(input, row - 1, col + 1));
        // Down Left
        sb.Append(CharAt(input, row + 1, col - 1));
        // Down Right
        sb.Append(CharAt(input, row + 1, col + 1));

        return sb.ToString();

    }

    private static char CharAt(string[] input, int row, int col)
    {
        if (row < 0 || row >= input.Length)
        {
            return '*';
        }

        if (col < 0 || col >= input[row].Length)
        {
            return '*';
        }

        return input[row][col];
    }
}
