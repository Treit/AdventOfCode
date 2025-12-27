using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using System.Transactions;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 06, CodeType.Original)]
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
        var answer = 0;
        var values = new List<int>();
        var ops = new List<string>();

        input = """
        123 328  51 64 
         45 64  387 23 
          6 98  215 314
        *   +   *   +  
        """.Split(Environment.NewLine);

        foreach (var line in input)
        {
            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Console.WriteLine($"{line} - {tokens.Length} tokens.");
            foreach (var token in tokens)
            {
                if (int.TryParse(token, out var num))
                {
                    values.Add(num);
                }
                else
                {
                    ops.Add(token);
                }
            }
        }

        Console.WriteLine(values.Count);
        Console.WriteLine(ops.Count);

        PrintColumn(values, ops, 0, 4);
        PrintColumn(values, ops, 1, 4);
        PrintColumn(values, ops, 2, 4);
        PrintColumn(values, ops, 3, 4);

        return answer.ToString();
    }

    public static string Part2(string[] input)
    {
        var answer = 0UL;

        return answer.ToString();
    }

    private static void PrintColumn(List<int> values, List<string> ops, int column, int stride)
    {
        Console.WriteLine("----");
        for (var i = column; i < values.Count; i += stride)
        {
            Console.WriteLine(values[i]);
        }

        var op = ops[column];
        Console.WriteLine(op);
    }
}
