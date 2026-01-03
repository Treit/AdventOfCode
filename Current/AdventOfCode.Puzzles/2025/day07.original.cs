using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 07, CodeType.Original)]
public class Day_07_Original : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = Part1(input.Lines);
        var part2 = Part2(input.Lines);
        return (part1, part2);
    }

    public static string Part1(string[] input)
    {
        var answer = 0L;

        /*
        input = """
        .......S.......
        ...............
        .......^.......
        ...............
        ......^.^......
        ...............
        .....^.^.^.....
        ...............
        ....^.^...^....
        ...............
        ...^.^...^.^...
        ...............
        ..^...^.....^..
        ...............
        .^.^.^.^.^...^.
        ...............
        """.Split(Environment.NewLine);
        */

        var stride = input[0].Length;
        var world = string.Concat(input).ToCharArray();
        var wh = new WorldHelper(world, stride);

        //wh.PrintWorld();

        var splitters = wh.GetSplitters();

        _ = wh.FireInitialBeam();

        //wh.PrintWorld();

        while (true)
        {
            foreach (var splitter in splitters)
            {
                splitter.CheckForBeamInput();
            }

            //wh.PrintWorld();
            var advanced = wh.AdvanceBeams();

            if (advanced == 0)
            {
                break;
            }
        }

        answer = splitters.Count(x => x.Splitting);
        Console.WriteLine($"Total splitter count: {splitters.Count}");

        return answer.ToString();
    }

    public static string Part2(string[] input)
    {
        var answer = BigInteger.Zero;

        /*
        input = """
        .......S.......
        ...............
        .......^.......
        ...............
        ......^.^......
        ...............
        .....^.^.^.....
        ...............
        ....^.^...^....
        ...............
        ...^.^...^.^...
        ...............
        ..^...^.....^..
        ...............
        .^.^.^.^.^...^.
        ...............
        """.Split(Environment.NewLine);
        */

        var stride = input[0].Length;
        var world = string.Concat(input).ToCharArray();
        var wh = new WorldHelper(world, stride);
        (_, var startCol) = wh.FireInitialBeam();

        wh.PrintWorld();
        answer = wh.TraverseAndAccumulate(startCol);

        return answer.ToString();
    }
}

internal class WorldHelper(char[] world, int stride)
{
    public char GetCharAt(int row, int col)
    {
        return world[col + (row * stride)];
    }

    public void SetCharAt(int row, int col, char value)
    {
        world[col + (row * stride)] = value;
    }

    public int GetRowCount()
    {
        return world.Length / stride;
    }

    public void PrintWorld()
    {
        Console.WriteLine("World:");
        for (var i = 0; i < world.Length; i++)
        {
            Console.Write(world[i]);

            if ((i + 1) % stride == 0)
            {
                Console.WriteLine();
            }
        }
    }

    public (int row, int col) FireInitialBeam()
    {
        for (var i = 0; i < GetRowCount(); i++)
        {
            for (var j = 0; j < stride; j++)
            {
                var c = GetCharAt(i, j);

                if (c == 'S')
                {
                    Console.WriteLine($"Firing beam from coordinates {(i, j)}");
                    var targetRow = i + 1;
                    SetCharAt(targetRow, j, '|');

                    return (i, j);
                }
            }
        }

        throw new InvalidOperationException("Unexpected state reached");
    }

    public BigInteger TraverseAndAccumulate(int startCol)
    {
        var rowCount = GetRowCount();
        var answer = BigInteger.Zero;

        // Skip first row and traverse the rest
        var currentRow = new BigInteger[stride];
        currentRow[startCol] = 1;
        var list = new List<BigInteger[]>();

        for (var i = 1; i < rowCount; i++)
        {
            var nextRow = new BigInteger[stride];

            for (var j = 0; j < stride; j++)
            {
                if (currentRow[j] == BigInteger.Zero)
                {
                    continue;
                }

                if (i + 1 == rowCount)
                {
                    // Leaving the grid. Calculate answer here.
                    foreach (var col in currentRow)
                    {
                        answer += col;
                    }
                    break;
                }

                var belowSymbol = GetCharAt(i + 1, j);

                if (belowSymbol == '.')
                {
                    nextRow[j] += currentRow[j];
                }
                else
                {
                    // Must be a splitter
                    nextRow[j - 1] += currentRow[j];
                    nextRow[j + 1] += currentRow[j];
                }

                list.Add(currentRow);
            }

            currentRow = nextRow;
        }

        return answer;
    }

    public int AdvanceBeams()
    {
        var rowCount = GetRowCount();
        int numberAdvanced = 0;

        for (var i = 0; i < rowCount; i++)
        {
            for (var j = 0; j < stride; j++)
            {
                var c = GetCharAt(i, j);

                if (c == '|')
                {
                    var targetRow = i + 1;
                    if (targetRow == rowCount)
                    {
                        continue;
                    }

                    var tileBelow = GetCharAt(targetRow, j);

                    if (tileBelow != '.')
                    {
                        continue;
                    }

                    Console.WriteLine($"Advancing beam from {(i, j)} to {(targetRow, j)}");
                    SetCharAt(targetRow, j, '|');
                    numberAdvanced++;
                }
            }
        }

        return numberAdvanced;

    }

    public List<Splitter> GetSplitters()
    {
        var result = new List<Splitter>();

        for (var i = 0; i < GetRowCount(); i++)
        {
            for (var j = 0; j < stride; j++)
            {
                var c = GetCharAt(i, j);

                if (c == '^')
                {
                    Console.WriteLine($"Tachyon splitter online at {(i, j)}!");
                    var splitter = new Splitter(i, j, this);
                    result.Add(splitter);
                }
            }
        }

        return result;
    }
}

internal sealed class Splitter(int row, int col, WorldHelper wh)
{
    internal bool Splitting { get; private set; }

    internal void CheckForBeamInput()
    {
        if (Splitting)
        {
            // Already splitting.
            return;
        }

        // Check cell directly above us.
        var rowToCheck = row - 1;
        var colToCheck = col;

        if (rowToCheck >= 0)
        {
            var c = wh.GetCharAt(rowToCheck, colToCheck);
            if (c == '|')
            {
                // Do split
                Console.WriteLine($"Splitting at {row}, {col}!");
                var lastRow = wh.GetRowCount() - 1;

                Splitting = true;

                if (row == lastRow)
                {
                    return;
                }

                wh.SetCharAt(row + 1, col - 1, '|');
                wh.SetCharAt(row + 1, col + 1, '|');
            }
        }

        Console.WriteLine($"{row}, {col}");
    }
}
