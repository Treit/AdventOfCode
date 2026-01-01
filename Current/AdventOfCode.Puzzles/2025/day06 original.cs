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
        var answer = 0L;
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

        var columnCount = ops.Count;

        for (var i = 0; i < columnCount; i++)
        {
            answer += CalculateColumn(values, ops, i, columnCount);
        }

        return answer.ToString();
    }

    public static string Part2(string[] input)
    {
        var answer = 0UL;
        var values = new List<string>();
        var ops = new List<string>();

        /*
        input = """
        123 328  51 64 
         45 64  387 23 
          6 98  215 314
        *   +   *   +  
        """.Split(Environment.NewLine);
        */

        var columnCount = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        var joined = string.Concat(input).ToCharArray();

        foreach (var line in input)
        {
            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                if (int.TryParse(token, out var num))
                {
                    values.Add(num.ToString());
                }
                else
                {
                    ops.Add(token);
                }
            }
        }

        var columnMap = new Dictionary<int, int>();

        for (var i = 0; i < columnCount; i++)
        {
            var subCount = SubColumnCount(values, i, columnCount);
            columnMap.Add(i, subCount);
        }

        // Treat each one-character lane as a column for the purpose of finding
        // which columns are separators
        var stride = input[0].Length;

        var separators = new BitArray(stride + 1);

        // Special case for the last blank column at the end.
        separators[separators.Length - 1] = true;

        for (var col = 0; col < stride; col++)
        {
            // Detect if it's only whitespace
            var onlyWhitespace = true;

            for (var i = col; i < joined.Length; i += stride)
            {
                if (joined[i] != ' ')
                {
                    onlyWhitespace = false;
                }
            }

            if (onlyWhitespace)
            {
                separators[col] = true;
            }

        }

        // Now walk the original input and replace any whitespace with a placeholder,
        // except for separator columns.
        var updatedInput = new List<string>(input.Length);

        foreach (var line in input)
        {
            var chars = line.ToCharArray();
            for (var j = 0; j < chars.Length; j++)
            {
                if (chars[j] == ' ' && !separators[j] && !line.Contains('*') && !line.Contains('+'))
                {
                    chars[j] = 'X';
                }
                else if (chars[j] == ' ' && !separators[j] && line.Contains('*'))
                {
                    chars[j] = '*';
                }
                else if (chars[j] == ' ' && !separators[j] && line.Contains('+'))
                {
                    chars[j] = '+';
                }
            }

            var sb = new StringBuilder();
            sb.Append(new string(chars));
            sb.Append(' ');
            updatedInput.Add(sb.ToString());
        }

        joined = string.Concat(updatedInput).ToCharArray();

        // Treat each one-character lane as a column for the purpose of finding
        // which columns are separators
        stride = updatedInput[0].Length;

        var digits = new List<int>();
        var vals = new List<int>();

        for (var col = 0; col < stride; col++)
        {
            digits.Clear();

            if (separators[col])
            {
                continue;
            }

            char op = ' ';

            for (var i = col; i < joined.Length; i += stride)
            {
                var c = joined[i];
                if (char.IsDigit(c))
                {
                    digits.Add((int)char.GetNumericValue(c));
                }
                else
                {
                    op = joined[i];
                }
            }

            var numStr = string.Concat(digits);
            var num = int.Parse(numStr);
            vals.Add(num);
        }

        Console.WriteLine($"Dumping column map: how many sub-columns does each primary column contain?");

        foreach (var kvp in columnMap)
        {
            Console.WriteLine($"{kvp.Key} -> {kvp.Value}");
        }

        var totalTaken = 0;

        for (var col = 0; col < columnCount; col++)
        {
            var toTake = columnMap[col];
            var toProcess = vals.Skip(totalTaken).Take(toTake);
            totalTaken += toTake;

            var op = ops[col];

            var subAnswer = 0UL;

            if (op == "*")
            {
                foreach (var v in toProcess)
                {
                    if (subAnswer == 0)
                    {
                        subAnswer = 1;
                    }

                    subAnswer *= (ulong)v;
                }
            }
            else
            {
                foreach (var v in toProcess)
                {
                    subAnswer += (ulong)v;
                }
            }

            Console.WriteLine($"Sub answer for column {col} is {subAnswer}");

            answer += subAnswer;
        }

        return answer.ToString();
    }

    private static int SubColumnCount(
        List<string> values,
        int column,
        int stride)
    {
        int longest = 0;

        for (var i = column; i < values.Count; i += stride)
        {
            var len = values[i].Length;

            if (len > longest)
            {
                longest = len;
            }
        }

        return longest;
    }

    private static long CalculateColumn(
        List<int> values,
        List<string> ops,
        int column,
        int stride)
    {
        var result = 0L;
        var op = ops[column];

        for (var i = column; i < values.Count; i += stride)
        {
            var val = values[i];

            if (op == "+")
            {
                result += val;
            }
            else
            {
                if (result == 0)
                {
                    result = 1;
                }

                result *= val;
            }
        }

        return result;
    }
}
