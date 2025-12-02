using System.Runtime.InteropServices;
using System.Transactions;

namespace AdventOfCode.Puzzles._2025;

[Puzzle(2025, 01, CodeType.Original)]
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
        var current = 50;
        var answer = 0;

        foreach (var line in input)
        {
            var dir = line[0];
            ReadOnlySpan<char> span = line.AsSpan();
            var len = int.Parse(span.Slice(1));
            var newValue = dir switch
            {
                'L' => Rotate(current, len, 100),
                'R' => Rotate(current, -len, 100),
                _ => throw new InvalidOperationException($"Unexpected input '{dir}'")
            };

            current = newValue;

            if (current == 0)
            {
                answer++;
            }
        }

        return answer.ToString();
    }

    public static string Part2(string[] input)
    {
        var answer = 0;

        var dial = CircularBuffer.Create(100);
        dial.RotateToTarget(50);

        foreach (var line in input)
        {
            var dir = line[0];
            ReadOnlySpan<char> span = line.AsSpan();
            var len = int.Parse(span.Slice(1));
            var newValue = dir switch
            {
                'L' => dial.RotateLeftAndCount(len),
                'R' => dial.RotateRightAndCount(len),
                _ => throw new InvalidOperationException($"Unexpected input '{dir}'")
            };

            answer += newValue;
        }

        return answer.ToString();

    }

    // Rotate left (positive amount) or right (negative amount) handling wrap-around.
    private static int Rotate(int pos, int amount, int totalSlots)
    {
        return ((pos - amount) % totalSlots + totalSlots) % totalSlots;
    }
}

internal class Node
{
    public int Value { get; set; }
    public Node? Prev { get; set; }
    public Node? Next { get; set; }
}

internal class CircularBuffer
{
    private Node Current;

    private CircularBuffer(Node initial)
    {
        Current = initial;
    }

    internal int RotateRightAndCount(int amount)
    {
        int clicks = 0;
        int result = 0;

        while (clicks < amount)
        {
            Current = Current.Next!;
            clicks++;

            if (Current.Value == 0)
            {
                result++;
            }
        }

        return result;
    }

    internal int RotateLeftAndCount(int amount)
    {
        int clicks = 0;
        int result = 0;

        while (clicks < amount)
        {
            Current = Current.Prev!;
            clicks++;

            if (Current.Value == 0)
            {
                result++;
            }
        }

        return result;
    }

    internal void RotateToTarget(int target)
    {
        if (Current == null)
        {
            throw new InvalidOperationException("Current is null!");
        }

        while (Current.Value != target)
        {
            Current = Current.Next!;
        }
    }

    internal static CircularBuffer Create(int size)
    {
        var firstNode = new Node();
        firstNode.Value = 0;
        firstNode.Prev = null;
        var prevNode = firstNode;

        for (var i = 1; i < size; i++)
        {
            var newNode = new Node();
            newNode.Value = i;
            newNode.Prev = prevNode;
            prevNode.Next = newNode;
            prevNode = newNode;
        }

        // Complete the circle
        firstNode.Prev = prevNode;
        prevNode.Next = firstNode;

        return new CircularBuffer(firstNode);
    }
}
