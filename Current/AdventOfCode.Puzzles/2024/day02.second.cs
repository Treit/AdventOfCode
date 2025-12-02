namespace AdventOfCode.Puzzles._2024;

[Puzzle(2024, 02, CodeType.Fastest)]
public class Day_02_Second : IPuzzle
{
    public (string, string) Solve(PuzzleInput input)
    {
        var part1 = SolvePart1(input.Lines);
        var part2 = SolvePart2(input.Lines);

        return (part1, part2);
    }

    public static string SolvePart1(string[] input)
    {
        var countOfSafe = 0;

        foreach (var line in input)
        {
            var numbers = ToNumbers(line);
            if (AllIncreasing(numbers))
            {
                countOfSafe++;
            }
            else if (AllDecreasing(numbers))
            {
                countOfSafe++;
            }
        }

        return countOfSafe.ToString();
    }

    public static string SolvePart2(string[] input)
    {
        var countOfSafe = 0;

        foreach (var line in input)
        {
            var numbers = ToNumbers(line);
            var swapBack = -1;
            var indexToRemove = numbers.Length;

            while (true)
            {
                indexToRemove--;

                if (AllIncreasing(numbers))
                {
                    countOfSafe++;
                    break;
                }
                else if (AllDecreasing(numbers))
                {
                    countOfSafe++;
                    break;
                }

                if (indexToRemove == -1)
                {
                    break;
                }

                if (swapBack != -1)
                {
                    numbers[indexToRemove + 1] = swapBack;
                }

                swapBack = numbers[indexToRemove];
                numbers[indexToRemove] = -1;
            }
        }

        return countOfSafe.ToString();
    }

    private static int[] ToNumbers(string input)
    {
        return input.Split(' ').Select(int.Parse).ToArray();
    }

    private static bool AllIncreasing(Span<int> input)
    {
        var last = -1;

        foreach (var number in input)
        {
            if (number == -1)
            {
                continue;
            }

            if (last == -1)
            {
                last = number - 1;
            }

            if (last < number && Math.Abs(number - last) <= 3)
            {
                last = number;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private static bool AllDecreasing(Span<int> input)
    {
        var last = -1;

        foreach (var number in input)
        {
            if (number == -1)
            {
                continue;
            }

            if (last == -1)
            {
                last = number + 1;
            }

            if (last > number && Math.Abs(number - last) <= 3)
            {
                last = number;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}
